using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;

        QuestList questList;

        private void Start()
        {
            questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.onQuestListUpdated += UpdateUI;
            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            foreach (QuestStatus status in questList.Statuses())
            {
                QuestItemUI uiInstance = Instantiate(questPrefab, transform);
                uiInstance.Setup(status);
            }
        }
    }
}
