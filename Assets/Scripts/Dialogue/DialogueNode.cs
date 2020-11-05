using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool isPlayerSpeaking = false; // TODO - make into an enum for multiple speakers
        [SerializeField] private string text = "";
        [SerializeField] private List<string> children = new List<string>();
        [SerializeField] private Rect rect = new Rect(20, 20, 200, 100);
        [SerializeField] private string onEnterAction = "";
        [SerializeField] private string onExitAction = "";
        [SerializeField] private Condition condition;

        public bool IsPlayerSpeaking() => isPlayerSpeaking;

        public string GetText() => text;
        
        public List<string> GetChrildren() => children;

        public Rect GetRect() => rect;

        public string GetOnEnterAction() => onEnterAction;

        public string GetOnExitAction() => onExitAction;

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators) => condition.Check(evaluators);

#if UNITY_EDITOR
        public void SetIsPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Update IsPlayerSpeaking");
            isPlayerSpeaking = newIsPlayerSpeaking;
        }

        public void SetText(string textToSet)
        {
            Undo.RecordObject(this, "Update dialogue text");
            text = textToSet;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childID)
        {

            Undo.RecordObject(this, "Link node");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Unlink node");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPosition(Vector2 position)
        {
            Undo.RecordObject(this, "Move node");
            rect.position = position;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
