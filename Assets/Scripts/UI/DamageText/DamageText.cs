using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;

        private static Vector3 startPos = new Vector3();
        private static Vector3 endPos = new Vector3(0f, 3f, 0f);
        private static float moveTime = 0.5f;
        private static float fadeDelay = 0.3f;
        private static float fadeTime = 0.2f;

        private DamageTextSpawner spawner = null;

        private void Awake()
        {
            if (damageText == null)
                damageText = GetComponent<TextMeshProUGUI>();

            startPos = transform.localPosition;
        }

        public void Init(DamageTextSpawner spawner, float textValue)
        {
            this.spawner = spawner;
            damageText.text = String.Format("{0:0}", textValue);

            StartFade();
        }

        private void StartFade()
        {
            LeanTween.moveLocal(gameObject, endPos, moveTime)
                     .setEaseInQuad();

            LeanTween.value(damageText.gameObject, 1f, 0f, fadeTime)
                     .setDelay(fadeDelay)
                     .setEaseInQuart()
                     .setOnUpdate(SetTextAlpha)
                     .setOnComplete(OnFadeComplete);
        }

        private void OnFadeComplete()
        {
            spawner.OnTextFadeComplete(this);

            Destroy(gameObject);
        }

        private void SetTextAlpha(float value)
        {
            damageText.alpha = value;
        }
    }
}
