using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueChoiceUI : MonoBehaviour
    {
        [SerializeField] private Button choiceButton;
        [SerializeField] private TextMeshProUGUI choiceText;

        public Button GetButton() => choiceButton;

        public void SetText(string text)
        {
            choiceText.text = text;
        }
    }
}
