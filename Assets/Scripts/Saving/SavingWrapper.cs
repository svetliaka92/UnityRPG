using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save01";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                GetComponent<SavingSystem>().Save(defaultSaveFile);

            if (Input.GetKeyDown(KeyCode.L))
                GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}
