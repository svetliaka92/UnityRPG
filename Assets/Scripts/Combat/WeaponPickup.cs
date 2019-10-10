using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private float respawnTime = 5f;

        private Collider collider;

        private WaitForSeconds hideDelay;

        private void Start()
        {
            hideDelay = new WaitForSeconds(respawnTime);
            collider = GetComponent<Collider>();
        }

        private void UpdatePickupRespawnTime(float respawnTime)
        {
            this.respawnTime = respawnTime;
            hideDelay = new WaitForSeconds(respawnTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                Fighter fighter = other.GetComponent<Fighter>();
                fighter.EquipWeapon(weapon);

                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            TogglePickup(false);

            yield return hideDelay;

            TogglePickup(true);
        }

        private void TogglePickup(bool flag)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(flag);

            collider.enabled = flag;
        }
    }
}
