using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 100)] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private Experience experience;
        [SerializeField] private GameObject levelUpParticles = null;

        private LazyValue<int> currentLevel;

        public event Action onLevelUpEvent;

        private void Awake()
        {
            experience = GetComponent<Experience>();

            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        public void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience)
                experience.onExperienceGained += UpdateLevel;
        }

        private void OnDisable()
        {
            if (experience)
                experience.onExperienceGained -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;

                if (onLevelUpEvent != null)
                    onLevelUpEvent();

                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticles, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveMultiplier(stat)) * GetPercentageMultiplier(stat);
        }

        public float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveMultiplier(Stat stat)
        {
            float total = 0f;

            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                    total += modifier;

            return total;
        }

        private float GetPercentageMultiplier(Stat stat)
        {
            float totalPercent = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                    totalPercent += modifier;

            return 1f + totalPercent / 100f;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        public int CalculateLevel()
        {
            if (experience == null)
                return startingLevel;

            float currentXP = experience.GetExperience();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                    return level;
            }

            return penultimateLevel + 1;
        }
    }
}
