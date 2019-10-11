using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float healthPoints = -1f;
        private BaseStats baseStats;

        public float GetHealth
        {
            get { return healthPoints; }
        }
        private bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
        }

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            if (healthPoints >= 0)
                return;

            healthPoints = baseStats.GetStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints -= damage;
            if (healthPoints <= 0f && !isDead)
            {
                healthPoints = 0f;
                Die();

                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            return (healthPoints / baseStats.GetStat(Stat.Health)) * 100f;
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("DeathTrigger");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();
            if (exp)
                exp.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            // restore health points
            // die if no HP left

            healthPoints = (float)state;
            if (healthPoints <= 0)
                Die();
        }
    }
}
