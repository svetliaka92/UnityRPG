using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Drop Library")]
    public class DropLibrary : ScriptableObject
    {
        // - drop chance
        // - min drops
        // - max drops
        // - potential drops
        //   - relative chance
        //   - min items
        //   - max items

        [SerializeField] float[] dropPercentChance;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;

        [SerializeField]
        DropConfig[] potentialDrops;

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;

            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable())
                    return 1;

                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(maxNumber, level);

                return Random.Range(min, max + 1);
            }
        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldDrop(level)) yield break;

            for (int i = 0; i < GetRandomNumberOfDrops(level); ++i)
                yield return GetRandomDrop(level);
        }

        bool ShouldDrop(int level)
        {
            return Random.Range(0, 100) < GetByLevel(dropPercentChance, level);
        }

        int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDrops, level);
            int max = GetByLevel(maxDrops, level);

            return Random.Range(min, max);
        }

        Dropped GetRandomDrop(int level)
        {
            DropConfig drop = SelectRandomItem(level);
            Dropped result = new Dropped();

            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);

            return result;
        }

        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0f;
            foreach (DropConfig drop in potentialDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChance, level);
                if (chanceTotal > randomRoll)
                    return drop;
            }

            return null;
        }

        private float GetTotalChance(int level)
        {
            float chance = 0f;
            foreach (DropConfig drop in potentialDrops)
                chance += GetByLevel(drop.relativeChance, level);

            return chance;
        }

        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
                return default;
            if (level > values.Length)
                return values[values.Length - 1];
            if (level <= 0)
                return default;

            return values[level - 1];
        }
    }
}
