using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            try
            {
                string path = GetPathFromSaveFile(saveFile);
                print("Saving to: " + path);
                using (FileStream stream = File.Open(path, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    bf.Serialize(stream, CaptureState());
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                print(e.Message);
            }
        }

        public void Load(string saveFile)
        {
            print("Loading from: " + GetPathFromSaveFile(saveFile));
            string path = GetPathFromSaveFile(saveFile);
            try
            {
                if (File.Exists(path))
                {
                    using (FileStream stream = File.Open(path, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        RestoreState(bf.Deserialize(stream));

                        stream.Close();
                    }
                }
            }
            catch(Exception e)
            {
                //..
            }
        }

        private object CaptureState()
        {
            Dictionary<string, object> gameState = new Dictionary<string, object>();

            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
                gameState[saveable.GetUniqueIdentifier()] = saveable.CaptureState();

            return gameState;
        }

        private void RestoreState(object state)
        {
            Dictionary<string, object> gameState = (Dictionary<string, object>)state;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
                saveable.RestoreState(gameState);
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }

    /// <summary>
    /// Helper classes
    /// </summary>
    [System.Serializable]
    public class SerializableVector3
    {
        float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}
