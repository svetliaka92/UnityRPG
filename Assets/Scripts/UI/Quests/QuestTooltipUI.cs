using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] QuestObjective questObjectivePrefab;
        [SerializeField] TextMeshProUGUI reward;

        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            // clear objective container
            foreach (Transform child in objectiveContainer)
                Destroy(child.gameObject);

            title.text = quest.Title();

            // fill objective container from quest
            foreach (Quest.Objective objective in quest.Objectives())
            {
                QuestObjective questObjectiveInstance = Instantiate(questObjectivePrefab, objectiveContainer);
                questObjectiveInstance.Setup(objective.description, status.IsObjectiveComplete(objective.reference));
            }

            reward.text = GetRewardText(quest);
        }

        private static string GetRewardText(Quest quest)
        {
            string rewardText = "";
            foreach (Quest.Reward reward in quest.Rewards())
            {
                if (!string.IsNullOrEmpty(rewardText))
                {
                    rewardText += ", ";
                }
                rewardText += ((reward.number > 1) ? reward.number.ToString() : "") + " " + reward.item.GetDisplayName();
            }
            if (string.IsNullOrEmpty(rewardText))
                rewardText = "No reward";

            rewardText += ".";

            return rewardText;
        }
    }
}
