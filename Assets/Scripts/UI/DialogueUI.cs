using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] Transform AIResponseRoot;
        [SerializeField] Transform choiceRoot;
        [SerializeField] DialogueChoiceUI choicePrefab;
        [SerializeField] Button quitButton;
        [SerializeField] TextMeshProUGUI conversantName;

        private void Start()
        {
            playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;

            if (nextButton)
                nextButton.onClick.AddListener(playerConversant.Next);

            if (quitButton)
                quitButton.onClick.AddListener(playerConversant.QuitDialogue);

            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());

            if (!playerConversant.IsActive())
                return;

            conversantName.text = playerConversant.GetCurrentConversantName();
            AIResponseRoot.gameObject.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform child in choiceRoot)
                Destroy(child.gameObject);

            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                DialogueChoiceUI dialogueChoice = Instantiate(choicePrefab, choiceRoot);
                dialogueChoice.SetText(choice.GetText());

                Button button = dialogueChoice.GetButton();
                button.onClick.AddListener(() =>
                {
                    playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}
