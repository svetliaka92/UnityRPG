using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private Animator animator;
        private NavMeshAgent agent;

        private int forwardSpeedId = Animator.StringToHash("ForwardSpeed");

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
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
