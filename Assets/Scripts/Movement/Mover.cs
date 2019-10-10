using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 5.662317f;

        private Animator animator;
        private NavMeshAgent agent;
        private ActionScheduler actionScheduler;
        private Health health;

        private int forwardSpeedId = Animator.StringToHash("ForwardSpeed");

        private void Awake()
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

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 pos = (SerializableVector3)state;

            agent.enabled = false;
            transform.position = pos.ToVector3();
            agent.enabled = true;

            actionScheduler.CancelCurrentAction();
        }
    }
}
