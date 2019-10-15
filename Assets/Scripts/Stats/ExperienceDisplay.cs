using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI experienceText;
        [SerializeField] private TextMeshProUGUI experienceToLevelText;
        [SerializeField] private RectTransform expBar;

        private Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            experienceText.text = experience.GetExperience().ToString();
            experienceToLevelText.text = experience.GetExperienceToLevel().ToString();

            float expToLevel = experience.GetExperienceToLevel();

            expBar.localScale = new Vector3(Mathf.Approximately(expToLevel, 0f) ? 0f : experience.GetFraction(),
                                            1f,
                                            1f);
        }
    }
}
