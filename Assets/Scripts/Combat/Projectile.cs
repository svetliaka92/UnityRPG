using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 0.2f;
        [SerializeField] Transform dummyTarget;

        private Transform currentTarget;

        private bool isInited;

        private void Start()
        {
            Init(dummyTarget);
        }

        public void Init(Transform target)
        {
            currentTarget = target;

            isInited = true;
        }

        private void Update()
        {
            if (!isInited)
                return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = currentTarget.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
                return currentTarget.position;

            return currentTarget.position + Vector3.up * (targetCapsule.height / 2);
        }
    }
}
