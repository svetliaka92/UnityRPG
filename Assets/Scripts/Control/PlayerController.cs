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
            if (InteractWithCombat())
                return;

            if (InteractWithLoot())
                return;

            if (InteractWithNPC())
                return;

            if (InteractWithMovement())
                return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if(Input.GetMouseButton(0))
                    fighter.Attack(target);

                return true;
            }

            return false;
        }

        private bool InteractWithLoot()
        {
            return false;
        }

        private bool InteractWithNPC()
        {
            return false;
        }

        private bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();
            RaycastHit hitInfo;

            bool hasHit = Physics.Raycast(ray, out hitInfo, 100f);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    mover.StartMoveAction(hitInfo.point);

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
