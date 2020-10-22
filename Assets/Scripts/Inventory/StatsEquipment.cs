using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventory
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (EquipLocation slot in GetAllPopulatedSlots())
            {
                IModifierProvider item = (IModifierProvider)GetItemInSlot(slot);
                if (item == null)
                    continue;

                foreach (float mod in item.GetAdditiveModifiers(stat))
                    yield return mod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            
            foreach (EquipLocation slot in GetAllPopulatedSlots())
            {
                IModifierProvider item = (IModifierProvider)GetItemInSlot(slot);
                if (item == null)
                    continue;

                foreach (float mod in item.GetPercentageModifiers(stat))
                    yield return mod;
            }
        }
    }
}
