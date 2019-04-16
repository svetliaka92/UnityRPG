using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private Animator animator;
        private NavMeshAgent agent;
        private Fighter fighter;

        private int forwardSpeedId = Animator.StringToHash("ForwardSpeed");

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void ToggleStop(bool flag)
        {
            agent.isStopped = flag;
        }

        public void StartMoveAction(Vector3 destination)
        {
            fighter.CancelAttack();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
            ToggleStop(false);
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
