using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionDuration = 3f;
        [SerializeField] private float aggroTime = 5f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellDuration = 5f;
        [SerializeField] private float shoutDistance = 5f;

        [Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.3f;
        [Range(0, 1)] [SerializeField] private float chaseSpeed = 0.8f;

        private Fighter fighter;
        private GameObject player;
        private Health health;
        private Mover mover;

        private LazyValue<Vector3> guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float timeSinceAggrevated = Mathf.Infinity;

        private int currentWaypointIndex = 0;
        

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player");

            guardPosition = new LazyValue<Vector3>(SetDefaultGuardPosition);
        }

        private Vector3 SetDefaultGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead)
                return;

            if (IsAggrevated()
                && fighter.CanAttack(player))
                AttackBehaviour();

            else if (timeSinceLastSawPlayer <= suspicionDuration)
                SuspicionBehaviour();

            else
                PatrolBehaviour();

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWaypoint >= waypointDwellDuration)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0f;
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0f);

            if (hits != null && hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    AIController enemy = hit.collider.GetComponent<AIController>();

                    if (enemy)
                        enemy.Aggrevate();
                }
            }
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer <= chaseDistance || timeSinceAggrevated < aggroTime;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
