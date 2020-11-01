using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
                nodeLookup[node.name] = node;
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.GetChrildren())
                if (nodeLookup.ContainsKey(childID))
                    yield return nodeLookup[childID];
        }

        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode createdNode = MakeNode(parentNode);

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(createdNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Create Dialogue Node");
#endif
            AddNode(createdNode);
        }

        private void AddNode(DialogueNode createdNode)
        {
            nodes.Add(createdNode);
            OnValidate();
        }

        private DialogueNode MakeNode(DialogueNode parentNode)
        {
            DialogueNode createdNode = CreateInstance<DialogueNode>();
            createdNode.name = Guid.NewGuid().ToString();

            if (parentNode != null)
            {
                parentNode.AddChild(createdNode.name);
                createdNode.SetIsPlayerSpeaking(!parentNode.IsPlayerSpeaking());

                createdNode.SetPosition(parentNode.GetRect().position + newNodeOffset);
            }

            return createdNode;
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Delete Dialogue Node");
#endif
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(nodeToDelete);
#endif
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
                node.RemoveChild(nodeToDelete.name);
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count <= 0)
            {
                DialogueNode createdNode = MakeNode(null);
                AddNode(createdNode);
            }

            if (!String.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (String.IsNullOrEmpty(AssetDatabase.GetAssetPath(node)))
                        AssetDatabase.AddObjectToAsset(node, this);
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}
