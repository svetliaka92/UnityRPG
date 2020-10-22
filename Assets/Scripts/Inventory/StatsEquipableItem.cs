using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Equipable Item")]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        [SerializeField] private Modifier[] additiveModifiers;
        [SerializeField] private Modifier[] percentageModifiers;

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (Modifier mod in additiveModifiers)
                if (mod.stat == stat)
                    yield return mod.value;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (Modifier mod in percentageModifiers)
                if (mod.stat == stat)
                    yield return mod.value;
        }
    }
}
