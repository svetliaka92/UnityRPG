using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
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
            if (currentWeapon == null)
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
                if (currentWeapon.HasProjectile())
                    currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
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
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            Weapon weapon = UnityEngine.Resources.Load<Weapon>((string)state);
            EquipWeapon(weapon);
        }
    }
}
