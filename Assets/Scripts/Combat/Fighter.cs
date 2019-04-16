using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;

        private Transform target;
        private Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (target == null)
                return;

            if (!InRange())
                mover.MoveTo(target.position);
            else
                mover.ToggleStop(true);
        }

        private bool InRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void CancelAttack()
        {
            target = null;
        }
    }
}
