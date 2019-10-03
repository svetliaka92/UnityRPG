using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        private void OnTriggerEnter(Collider other)
        {
            print("Collided with: " + other + ", tag: " + other.tag);
            if (other.tag.Equals("Player"))
            {
                Fighter fighter = other.GetComponent<Fighter>();
                fighter.EquipWeapon(weapon);

                Destroy(gameObject);
            }
        }
    }
}
