using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float healthPoints = 100f;
        public float GetHealth
        {
            get { return healthPoints; }
        }
        private bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
        }

        public void TakeDamage(float damage)
        {
            healthPoints -= damage;
            if (healthPoints <= 0f && !isDead)
            {
                isDead = true;
                healthPoints = 0f;
                GetComponent<Animator>().SetTrigger("DeathTrigger");
            }
        }
    }
}
