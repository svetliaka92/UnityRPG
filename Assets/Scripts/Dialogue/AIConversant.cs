using GameDevTV.Inventories;
using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Dialogue AIDialogue = null;
        [SerializeField] private string AIName = "Harold the Herald Stacker";

        [SerializeField] InventoryItem itemToTake;
        [SerializeField] int numberOfItems;

        public string GetAIName() => AIName;

        PlayerController player;
        GameDevTV.Inventories.Inventory playerInventory;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (AIDialogue == null)
            {
                player = null;
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                player = callingController;
                player.GetComponent<PlayerConversant>().StartDialogue(AIDialogue, this);
                playerInventory = player.GetComponent<GameDevTV.Inventories.Inventory>();
            }

            return true;
        }

        public void GiveItem()
        {
            //print($"NPC {AIName} taking item {numberOfItems}x{itemToTake.GetDisplayName()}");
            if (playerInventory.HasItem(itemToTake))
            {
                //print($"Player has item {itemToTake.GetDisplayName()}");
                if (playerInventory.RemoveItem(itemToTake, numberOfItems))
                {
                    //print("Player giving item to NPC successful");
                }
            }
        }
    }
}
