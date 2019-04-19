using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private void Update()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (Vector3.Distance(player.transform.position, transform.position) <= chaseDistance)
                print(gameObject + "is rolling out!");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
