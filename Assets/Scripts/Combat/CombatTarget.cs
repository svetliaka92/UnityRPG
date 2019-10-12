﻿using RPG.Control;
using RPG.Resources;
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
            if (!callingController.PlayerFighter.CanAttack(gameObject))
                return false;

            if (Input.GetMouseButton(0))
                callingController.PlayerFighter.Attack(gameObject);

            return true;
        }
    }
}
