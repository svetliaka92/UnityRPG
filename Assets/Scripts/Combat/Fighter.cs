using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;

        [SerializeField] Weapon defaultWeapon = null;

        private float timeSinceLastAttack = Mathf.Infinity;
        private Animator animator;
        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;

        private float range = 2f;
        private float timeBetweenAttacks = 1f;
        private float damage = 5f;

        private Weapon currentWeapon = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            EquipWeapon(defaultWeapon);
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
            if (weapon == null)
                return;

            Animator anim = GetComponent<Animator>();
            currentWeapon = weapon;
            currentWeapon.Spawn(rightHandTransform, leftHandTransform, anim);

            range = currentWeapon.GetRange();
            timeBetweenAttacks = currentWeapon.GetTimeBetweenAttacks();
            damage = currentWeapon.GetDamage();
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
                if (currentWeapon.HasProjectile())
                    currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
                else
                    target.TakeDamage(damage);
            }
        }

        public void Shoot()
        {
            Hit();
        }
    }
}
