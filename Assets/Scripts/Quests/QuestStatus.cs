using RPG.UI.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives = new List<string>();
        }

        public Quest GetQuest() => quest;
        public List<string> GetCompletedObjectives() => completedObjectives;

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord questStatusRecord = objectState as QuestStatusRecord;
            if (questStatusRecord != null)
            {
                quest = Quest.GetByName(questStatusRecord.questName);
                completedObjectives = questStatusRecord.completedObjectives;
            }
        }

        public bool IsComplete()
        {
            foreach (Quest.Objective objective in quest.Objectives())
                if (!completedObjectives.Contains(objective.reference))
                    return false;

            return true;
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
                completedObjectives.Add(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord questStatusRecord = new QuestStatusRecord();
            questStatusRecord.questName = quest.name;
            questStatusRecord.completedObjectives = completedObjectives;

            return questStatusRecord;
        }
    }
}
