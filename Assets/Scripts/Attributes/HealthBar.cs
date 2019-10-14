using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health = null;
        [SerializeField] private RectTransform foreground = null;
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private bool shoundHide = true;
        private void Update()
        {
            float healthFraction = health.GetFraction();

            if (shoundHide)
            {
                if (Mathf.Approximately(healthFraction, 0f)
                  || Mathf.Approximately(healthFraction, 1f))
                {
                    rootCanvas.enabled = false;
                }
                else
                    rootCanvas.enabled = true;
            }

            foreground.localScale = new Vector3(health.GetFraction(), 1f, 1f);
        }
    }
}
