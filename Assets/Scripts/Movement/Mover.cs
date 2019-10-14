using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 5.662317f;
        [SerializeField] private float maxPathLength = 40f;

        private Animator animator;
        private NavMeshAgent agent;
        private ActionScheduler actionScheduler;
        private Health health;

        private int forwardSpeedId = Animator.StringToHash("ForwardSpeed");

        private bool isInited = false;

        private void Awake()
        {
            if (!isInited)
                Init();
        }

        private void Init()
        {
            agent = GetComponent<NavMeshAgent>();

            animator = GetComponent<Animator>();

            actionScheduler = GetComponent<ActionScheduler>();

            health = GetComponent<Health>();

            isInited = true;
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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath)
                return false;

            if (path.status != NavMeshPathStatus.PathComplete)
                return false;

            if (GetPathLength(path) > maxPathLength)
                return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float totalDistance = 0f;

            if (path.corners.Length < 2)
                return totalDistance;

            Vector3[] points = path.corners;

            for (int i = 0; i < points.Length - 1; i++)
                totalDistance += Vector3.Distance(points[i + 1], points[i]);

            return totalDistance;
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
            if (!isInited)
                Init();

            SerializableVector3 pos = (SerializableVector3)state;

            agent.enabled = false;
            transform.position = pos.ToVector3();
            agent.enabled = true;

            actionScheduler.CancelCurrentAction();
        }
    }
}
