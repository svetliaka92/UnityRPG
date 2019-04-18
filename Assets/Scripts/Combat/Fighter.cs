using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponDamage = 5f;

        private float timeSinceLastAttack = 0f;
        private Animator animator;
        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;

        private void Start()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
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

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null)
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
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void CancelAttack()
        {
            TriggerAttack(false);
            target = null;
        }

        public void Hit()
        {
            if(target)
                target.TakeDamage(weaponDamage);
        }
    }
}
