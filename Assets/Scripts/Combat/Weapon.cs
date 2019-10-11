using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/ Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private bool isRightHanded = true;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 5f;

        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand,
                          Transform leftHand,
                          Animator animator)
        {
            DestroyOldWeapong(rightHand, leftHand);

            if (weaponPrefab)
            {
                GameObject weapon = Instantiate(weaponPrefab, GetTransform(rightHand, leftHand));
                weapon.name = weaponName;
            }

            AnimatorOverrideController overrideController =
                    animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride)
                animator.runtimeAnimatorController = animatorOverride;
            
            else if(overrideController)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        private void DestroyOldWeapong(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
                oldWeapon = leftHand.Find(weaponName);

            if (oldWeapon == null)
                return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public Transform GetTransform(Transform rightHand,
                                      Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand,
                                     Transform leftHand,
                                     Health health,
                                     GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile,
                                                        GetTransform(rightHand, leftHand).position,
                                                        Quaternion.identity);

            projectileInstance.SetTarget(health, instigator, weaponDamage);
        }
    }
}
