using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isIntroPlayed = false;
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag.Equals("Player") && !isIntroPlayed)
            {
                isIntroPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
