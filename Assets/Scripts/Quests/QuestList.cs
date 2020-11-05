using GameDevTV.Inventories;
using RPG.Core;
using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public List<QuestStatus> Statuses() => statuses;

        public event Action onQuestListUpdated;

        private GameDevTV.Inventories.Inventory playerInventory;
        private ItemDropper itemDropper;

        private void Awake()
        {
            playerInventory = GetComponent<GameDevTV.Inventories.Inventory>();
            itemDropper = GetComponent<ItemDropper>();
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest))
                return;

            QuestStatus newStatus = new QuestStatus(quest);

            statuses.Add(newStatus);
            onQuestListUpdated?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            if (status != null)
            {
                status.CompleteObjective(objective);
                if (status.IsComplete())
                    GiveReward(quest);
  
                onQuestListUpdated?.Invoke();
            }
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
                if (status.GetQuest() == quest)
                    return status;

            return null;
        }

        private void GiveReward(Quest quest)
        {
            foreach (Quest.Reward reward in quest.Rewards())
            {
                bool success = playerInventory.AddToFirstEmptySlot(reward.item, reward.number);
                if (!success)
                    itemDropper.DropItem(reward.item, reward.number);
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus status in statuses)
                state.Add(status.CaptureState());

            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null)
                return;

            statuses.Clear();
            foreach (object objectState in stateList)
            {
                QuestStatus status = new QuestStatus(objectState);
                statuses.Add(status);
            }
        }

        public bool? Evaluate(string predicate, string[] paremeters)
        {
            switch (predicate)
            {
                case "HasQuest":
                    return HasQuest(Quest.GetByName(paremeters[0]));
                case "CompletedQuest":
                    return GetQuestStatus(Quest.GetByName(paremeters[0])).IsComplete();
            }

            return null;
        }
    }
}
