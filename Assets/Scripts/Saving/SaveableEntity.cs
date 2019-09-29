using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    [System.Serializable]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier()
        {
            return "";
        }

        public object CaptureState()
        {
            print("Capturing state for: " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            print("Restoring state for: " + GetUniqueIdentifier());
        }

        private void Update()
        {
            if (Application.IsPlaying(gameObject))
                return;

            if (string.IsNullOrEmpty(gameObject.scene.path))
                return;

            SerializedObject obj = new SerializedObject(this);
            SerializedProperty property = obj.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                obj.ApplyModifiedProperties();
            }
        }
    }
}
