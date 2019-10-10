using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 0.2f;
        [SerializeField] private bool isHoming = false;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float maxLifetime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2f;
        Health target = null;

        private float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (isHoming && !target.IsDead)
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifetime);
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

                if (hitEffect)
                {
                    GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
                    Destroy(hit, maxLifetime);
                }

                targetHit.TakeDamage(damage);

                foreach (GameObject toDestroy in destroyOnHit)
                    Destroy(toDestroy);

                Destroy(gameObject, lifeAfterImpact);
            }
        }
    }
}
