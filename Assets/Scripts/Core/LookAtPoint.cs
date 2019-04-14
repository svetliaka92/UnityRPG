using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [ExecuteInEditMode]
    public class LookAtPoint : MonoBehaviour
    {
        [SerializeField] private Vector3 lookAtPoint = Vector3.zero;

        void Update()
        {
            lookAtPoint = gameObject.transform.parent.transform.position;
            transform.LookAt(lookAtPoint);
        }
    }
}
