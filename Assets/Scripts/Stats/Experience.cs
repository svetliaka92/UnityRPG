using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0f;

        private BaseStats baseStats;

        public event Action onExperienceGained;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained?.Invoke();
        }

        public float GetExperienceToLevel()
        {
            return baseStats.GetStat(Stat.ExperienceToLevelUp);
        }

        public float GetFraction()
        {
            return experiencePoints / baseStats.GetStat(Stat.ExperienceToLevelUp);
        }

        public float GetExperience()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
