using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.UI.DamageText;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regeneratePercent = 70f;
        [SerializeField] DamageTextSpawner damageTextSpawner;

        private LazyValue<float> healthPoints;
        private BaseStats baseStats;

        public float GetHealth
        {
            get { return healthPoints.value; }
        }
        private bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
        }

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            baseStats.onLevelUpEvent += RegenerateHealth;
        }

        private void OnDestroy()
        {
            baseStats.onLevelUpEvent -= RegenerateHealth;
        }

        public void RegenerateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * (regeneratePercent / 100f);

            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value -= damage;

            if (healthPoints.value <= 0f && !isDead)
            {
                healthPoints.value = 0f;
                Die();

                AwardExperience(instigator);
            }
            else
                damageTextSpawner.Spawn(damage);
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return GetFraction() * 100f;
        }

        public float GetFraction()
        {
            return (healthPoints.value / baseStats.GetStat(Stat.Health));
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
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            // restore health points
            // die if no HP left

            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
                Die();
        }
    }
}
