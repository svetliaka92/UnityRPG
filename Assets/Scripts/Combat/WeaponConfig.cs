using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/ Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private Weapon weaponPrefab = null;
        [SerializeField] private bool isRightHanded = true;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float percentageBonus = 0f;

        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand,
                          Transform leftHand,
                          Animator animator)
        {
            DestroyOldWeapong(rightHand, leftHand);

            Weapon weapon = null;

            if (weaponPrefab)
            {
                weapon = Instantiate(weaponPrefab, GetTransform(rightHand, leftHand));
                weapon.gameObject.name = weaponName;
            }

            AnimatorOverrideController overrideController =
                    animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride)
                animator.runtimeAnimatorController = animatorOverride;
            
            else if(overrideController)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

            return weapon;
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

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand,
                                     Transform leftHand,
                                     Health health,
                                     GameObject instigator,
                                     float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile,
                                                        GetTransform(rightHand, leftHand).position,
                                                        Quaternion.identity);

            projectileInstance.SetTarget(health, instigator, calculatedDamage);
        }
    }
}
