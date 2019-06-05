using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad;

        private void OnTriggerEnter(Collider other)
        {
            print("Portal entered...");
            if (other.tag == "Player")
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }
}
