using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover mover;
        [SerializeField] private Fighter fighter;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            InteractWithCombat();
            InteractWithMovement();
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if(Input.GetMouseButton(0))
                    fighter.Attack(target);
            }
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
                MoveToCursor();
        }

        public void MoveToCursor()
        {
            Ray ray = GetMouseRay();
            RaycastHit hitInfo;

            bool hasHit = Physics.Raycast(ray, out hitInfo, 100f);
            if (hasHit)
                mover.MoveTo(hitInfo.point);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
