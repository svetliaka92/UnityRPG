using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI questTitle;
        [SerializeField] TextMeshProUGUI questProgress;
        [SerializeField] Color activeColor;
        [SerializeField] Color completedColor;

        QuestStatus status;

        public void Setup(QuestStatus status)
        {
            this.status = status;
            questTitle.text = status.GetQuest().Title();
            questProgress.text = status.GetCompletedObjectives().Count + "/" + status.GetQuest().Objectives().Count;
            questTitle.color = (status.IsComplete()) ? completedColor : activeColor;
            questProgress.color = (status.IsComplete()) ? completedColor : activeColor;
        }

        public QuestStatus GetStatus()
        {
            return status;
        }
    } 
}
