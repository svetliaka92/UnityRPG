using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        public void GiveQuest()
        {
            QuestList player = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            player.AddQuest(quest);
        }
    }
}
