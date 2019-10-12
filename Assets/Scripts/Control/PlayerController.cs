using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover mover;
        [SerializeField] private Fighter fighter;
        [SerializeField] private float maxNavMeshProjectionDistance = 100f;
        [SerializeField] private float maxPathLength = 40f;

        public Fighter PlayerFighter { get { return fighter; } }
        private Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D cursorTexture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI())
                return;

            if (health.IsDead)
                return;

            if (InteractWithComponent())
                return;

            if (InteractWithLoot())
                return;

            if (InteractWithNPC())
                return;

            if (InteractWithMovement())
                return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            // sort by distance
            // return
            return hits;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
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
            Vector3 target;
            bool hasHit = RaycastNavmesh(out target);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    mover.StartMoveAction(target);

                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();

            // raycast to terrain
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit)
                return false;

            // find nearest navmesh point
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point,
                                                           out navMeshHit,
                                                           maxNavMeshProjectionDistance,
                                                           NavMesh.AllAreas);

            if (!hasCastToNavMesh)
                return false;
            
            // return true if found
            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath)
                return false;

            if (path.status != NavMeshPathStatus.PathComplete)
                return false;

            if (GetPathLength(path) > maxPathLength)
                return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float totalDistance = 0f;

            if (path.corners.Length < 2)
                return totalDistance;

            Vector3[] points = path.corners;

            for (int i = 0; i < points.Length - 1; i++)
                totalDistance += Vector3.Distance(points[i + 1], points[i]);

            return totalDistance;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.cursorTexture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
                if (mapping.cursorType == type)
                    return mapping;

            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
