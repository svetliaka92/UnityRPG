using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventory
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far can the pickups be scattered from the dropper")]
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] DropLibrary dropLibrary;

        // constants
        const int ATTEMPTS = 30;

        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        public void RandomDrop()
        {
            IEnumerable<DropLibrary.Dropped> drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
            
            foreach (var drop in drops)
                DropItem(drop.item, drop.number);
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; ++i)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return transform.position;
        }
    }
}
