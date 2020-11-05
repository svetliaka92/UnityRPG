using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] string title;
        [SerializeField] string progress;
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }

        [System.Serializable]
        public class Reward
        {
            [Min(1)] public int number = 1;
            public InventoryItem item;
        }

        public string Title() => title;
        public string Progress() => progress;
        public List<Objective> Objectives() => objectives;
        public int GetObjectiveCount() => objectives.Count;
        public List<Reward> Rewards() => rewards;

        public bool HasObjective(string objectiveRef)
        {
            foreach (Objective objective in objectives)
                if (objective.reference.Equals(objectiveRef))
                    return true;

            return false;
        }

        public static Quest GetByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
                if (quest.name == questName)
                    return quest;

            return null;
        }
    }
}
