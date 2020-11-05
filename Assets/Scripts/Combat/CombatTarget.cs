using RPG.Control;
using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!enabled)
                return false;
             
            if (!callingController.PlayerFighter.CanAttack(gameObject))
                return false;

            if (Input.GetMouseButton(0))
                callingController.PlayerFighter.Attack(gameObject);

            return true;
        }
    }
}
