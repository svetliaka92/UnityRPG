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
        [SerializeField] private TakeDamageEvent onTakeDamage;
        [SerializeField] private UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        private LazyValue<float> healthPoints;
        private BaseStats baseStats;

        private float currentMaxHp = 0f;

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
            float newMax = baseStats.GetStat(Stat.Health);

            if (Mathf.Approximately(healthPoints.value, currentMaxHp))
                healthPoints.value = newMax;

            else
            {
                float regenHealthPoints = newMax * (regeneratePercent / 100f);

                healthPoints.value = Mathf.Min(healthPoints.value + regenHealthPoints, newMax);

                //healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
            }

            currentMaxHp = newMax;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value -= damage;

            if (healthPoints.value <= 0f && !isDead)
            {
                healthPoints.value = 0f;
                Die();

                onDie.Invoke();

                AwardExperience(instigator);
            }
            else
                onTakeDamage.Invoke(damage);
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

            currentMaxHp = baseStats.GetStat(Stat.Health);

            if (healthPoints.value <= 0)
                Die();
        }
    }
}
