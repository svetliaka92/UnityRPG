using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad;

        private void OnTriggerEnter(Collider other)
        {
            print("Portal entered...");
            if (other.tag == "Player")
                StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            print("Scene loaded");
            Destroy(gameObject);
        }
    }
}
