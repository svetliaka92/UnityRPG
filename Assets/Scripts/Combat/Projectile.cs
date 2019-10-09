using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 0.2f;
        [SerializeField] private bool isHoming = false;
        Health target = null;

        private float damage = 0;

        private bool isInited;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (!isInited)
                return;

            if (isHoming && !target.IsDead)
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.transform.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
                return target.transform.position;

            return target.transform.position + Vector3.up * (targetCapsule.height / 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            Health targetHit = other.GetComponent<Health>();
            if (targetHit != null && targetHit == target)
            {
                if (target.IsDead)
                    return;

                targetHit.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
