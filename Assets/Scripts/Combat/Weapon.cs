using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private UnityEvent onHitEvent;

        public void OnHit()
        {
            onHitEvent.Invoke();
        }
    }
}
