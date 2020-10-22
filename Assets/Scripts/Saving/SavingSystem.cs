using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private void Start()
        {
            //print(Application.persistentDataPath);
        }

        public IEnumerator LoadLastScene(string saveFile)
        {
            // get the state
            Dictionary<string, object> state = LoadFile(saveFile);
            int lastScene = SceneManager.GetActiveScene().buildIndex;

            if (state.ContainsKey("lastSceneBuildIndex"))
                lastScene = (int)state["lastSceneBuildIndex"];

            // load last scene
            yield return SceneManager.LoadSceneAsync(lastScene);

            // restore state
            RestoreState(state);
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);

            CaptureState(state);

            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private void SaveFile(string saveFile, Dictionary<string, object> state)
        {
            string path = GetPathFromSaveFile(saveFile);

            using (FileStream file = File.Open(path, FileMode.Create))
            {
                BinaryFormatter br = new BinaryFormatter();

                br.Serialize(file, state);
                file.Close();
            }

        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            else
            {
                using (FileStream file = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter br = new BinaryFormatter();

                    return (Dictionary<string, object>)br.Deserialize(file);
                }
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                    saveable.RestoreState(state[id]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
