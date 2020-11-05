using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue testDialogue;
        [SerializeField] private string playerName = "Player";

        private Dialogue currentDialogue;

        private DialogueNode currentNode = null;
        private bool isChoosing = false;

        private AIConversant currentConversant = null;

        public event Action onConversationUpdated;

        private IPredicateEvaluator[] predicateEvaluators;

        private void Awake()
        {
            predicateEvaluators = GetComponents<IPredicateEvaluator>();
        }

        public void StartDialogue(Dialogue newDialogue, AIConversant conversant)
        {
            currentConversant = conversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();

            TriggerEnterAction();

            onConversationUpdated?.Invoke();
        }

        public void QuitDialogue()
        {
            currentDialogue = null;

            TriggerExitAction();

            currentConversant = null;
            currentNode = null;
            isChoosing = false;

            onConversationUpdated?.Invoke();
        }

        public bool IsActive() => currentDialogue != null;

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
                return "";

            return currentNode.GetText();
        }

        internal string GetCurrentConversantName()
        {
            return (IsChoosing()) ? playerName : currentConversant.GetAIName();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChoices(currentNode));
        }

        public void SelectChoice(DialogueNode choice)
        {
            currentNode = choice;
            TriggerEnterAction();

            isChoosing = false;

            if (!currentDialogue.DisplayPlayerNodeTextOnChoice())
                Next();
            else
                onConversationUpdated?.Invoke();
        }

        public void Next()
        {
            if (UpdateIsChoosing())
            {
                onConversationUpdated?.Invoke();
                return;
            }

            IEnumerable<DialogueNode> enumerable = FilterOnCondition(currentDialogue.GetAllAIChildren(currentNode));
            List<DialogueNode> childNodes = new List<DialogueNode>();
            foreach (DialogueNode node in enumerable)
                childNodes.Add(node);

            int nextIndex = RollNextIndex();

            TriggerExitAction();
            currentNode = childNodes[nextIndex];
            TriggerEnterAction();

            onConversationUpdated?.Invoke();
        }

        private bool UpdateIsChoosing()
        {
            IEnumerable<DialogueNode> playerChoices = FilterOnCondition(currentDialogue.GetPlayerChoices(currentNode));
            int numPlayerChoices = 0;
            foreach (DialogueNode node in playerChoices)
                ++numPlayerChoices;

            if (numPlayerChoices > 0)
            {
                isChoosing = true;
                TriggerExitAction();
            }
            else
                isChoosing = false;

            return isChoosing;
        }

        private int RollNextIndex()
        {
            return UnityEngine.Random.Range(0, currentNode.GetChrildren().Count);
        }

        public bool HasNext()
        {
            IEnumerable<DialogueNode> nodes = FilterOnCondition(currentDialogue.GetAllChildren(currentNode));
            int numNodes = 0;
            foreach (DialogueNode node in nodes)
                ++numNodes;

            return numNodes > 0;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (DialogueNode node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return predicateEvaluators;
        }

        private void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (string.IsNullOrEmpty(action))
                return;

            DialogueTrigger[] triggers = currentConversant.GetComponents<DialogueTrigger>();
            foreach (DialogueTrigger trigger in triggers)
                trigger.Trigger(action);
        }
    }
}
