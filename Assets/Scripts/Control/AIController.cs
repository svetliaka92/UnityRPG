using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private Fighter fighter;
        private GameObject player;
        private Health health;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (health.IsDead)
                return;

            if (IsPlayerInRange()
                && fighter.CanAttack(player))
                fighter.Attack(player);
            else
                fighter.Cancel();
        }

        private bool IsPlayerInRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) <= chaseDistance;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
