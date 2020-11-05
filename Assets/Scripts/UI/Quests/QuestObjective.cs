using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Quests
{
    public class QuestObjective : MonoBehaviour
    {
        [SerializeField] private Image checkmarkImage;
        [SerializeField] private TextMeshProUGUI objectiveText;

        public void Setup(string text, bool isComplete = false)
        {
            objectiveText.text = text;
            checkmarkImage.enabled = isComplete;
        }
    }
}
