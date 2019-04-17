using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;

        private Animator animator;
        private Transform target;
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
            if (target == null)
                return;
            
            if (!InRange())
                mover.MoveTo(target.position);
            else
            {
                mover.ToggleStop(true);
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            animator.SetTrigger("AttackTrigger");
        }

        public void Cancel()
        {
            CancelAttack();
        }

        private bool InRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void CancelAttack()
        {
            target = null;
        }

        public void Hit()
        {
            print("Hit landed");
        }
    }
}
