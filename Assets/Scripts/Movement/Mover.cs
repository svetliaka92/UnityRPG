using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 5.662317f;

        private Animator animator;
        private NavMeshAgent agent;
        private ActionScheduler actionScheduler;
        private Health health;

        private int forwardSpeedId = Animator.StringToHash("ForwardSpeed");

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            agent.enabled = !health.IsDead;

            UpdateAnimator();
        }

        public void ToggleStop(bool flag)
        {
            agent.isStopped = flag;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction = 1f)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction = 1f)
        {
            agent.SetDestination(destination);
            agent.speed = maxSpeed * speedFraction;
            ToggleStop(false);
        }

        public void Cancel()
        {
            ToggleStop(true);
        }

        public void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            animator.SetFloat(forwardSpeedId, speed);
        }
    }
}
