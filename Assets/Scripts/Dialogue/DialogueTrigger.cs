using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private string action;
        [SerializeField] UnityEvent onTrigger;

        public void Trigger(string actionToTrigger)
        {
            if (actionToTrigger.Equals(action))
                onTrigger.Invoke();
        }
    }
}
