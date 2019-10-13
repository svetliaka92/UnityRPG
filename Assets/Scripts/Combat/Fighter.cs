using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;

        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private string defaultWeaponName = "Unarmed";

        private float timeSinceLastAttack = Mathf.Infinity;
        private Animator animator;
        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private BaseStats baseStats;

        private float range = 2f;
        private float timeBetweenAttacks = 1f;
        private float weaponDamage = 5f;

        private LazyValue<Weapon> currentWeapon;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            baseStats = GetComponent<BaseStats>();

            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null)
                return;

            if (target.IsDead)
                return;
            
            if (!InRange())
                mover.MoveTo(target.transform.position);
            else
            {
                mover.ToggleStop(true);
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator anim = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, anim);

            range = weapon.GetRange();
            timeBetweenAttacks = weapon.GetTimeBetweenAttacks();
            weaponDamage = weapon.GetDamage();
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack(true);
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack(bool isAttacking)
        {
            animator.ResetTrigger((isAttacking) ? "StopAttack" : "AttackTrigger");
            animator.SetTrigger((isAttacking) ? "AttackTrigger" : "StopAttack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null
                || combatTarget == this.gameObject)
                return false;

            Health test = combatTarget.GetComponent<Health>();
            if (test && !test.IsDead)
                return true;

            return false;
        }

        public void Cancel()
        {
            CancelAttack();
        }

        private bool InRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < range;
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void CancelAttack()
        {
            TriggerAttack(false);
            target = null;
            mover.Cancel();
        }

        public void Hit()
        {
            if (target)
            {
                float damage = baseStats.GetStat(Stat.Damage);

                if (currentWeapon.value.HasProjectile())
                    currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
                else
                    target.TakeDamage(gameObject, damage);
            }
        }

        public void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = UnityEngine.Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return currentWeapon.value.GetDamage();
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return currentWeapon.value.GetPercentageBonus();
        }
    }
}
