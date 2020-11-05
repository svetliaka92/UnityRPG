using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;

        [SerializeField] QuestsToComplete[] questsToComplete;

        [System.Serializable]
        private class QuestsToComplete
        {
            public Quest quest;
            public string objective;
        }

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
        }

        public void CompleteSelectedObjective(string objective)
        {
            QuestList questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            
            foreach (QuestsToComplete questToComplete in questsToComplete)
            {
                if (questToComplete.quest.HasObjective(objective))
                {
                    questList.CompleteObjective(questToComplete.quest, objective);
                    return;
                }
            }
        }
    }
}
