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
        [SerializeField] private float healthPoints = 100f;
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
            healthPoints = baseStats.GetHealth();
        }

        public void TakeDamage(float damage)
        {
            healthPoints -= damage;
            if (healthPoints <= 0f && !isDead)
            {
                healthPoints = 0f;
                Die();
            }
        }

        public float GetPercentage()
        {
            return (healthPoints / baseStats.GetHealth()) * 100f;
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("DeathTrigger");
            GetComponent<ActionScheduler>().CancelCurrentAction();
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
