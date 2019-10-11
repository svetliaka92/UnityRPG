using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] progressionCharacterClasses;

        public float GetStat(Stat stat ,CharacterClass characterClass, int level)
        {
            //foreach (ProgressionCharacterClass progressionCharacterClass in progressionCharacterClasses)
            //    if (progressionCharacterClass.characterClass == characterClass)
            //        return progressionCharacterClass.health[level - 1];

            foreach (ProgressionCharacterClass progressionCharacterClass in progressionCharacterClasses)
            {
                if (progressionCharacterClass.characterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in progressionCharacterClass.stats)
                {
                    if (progressionStat.stat != stat) continue;

                    if (progressionStat.levels.Length < level) continue;

                    return progressionStat.levels[level - 1];
                }
            }

            return 50f;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
