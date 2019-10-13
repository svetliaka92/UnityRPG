using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab;

        private Queue<DamageText> damageTextPool;
        private int maxDamageTexts = 20;

        public void Spawn(float value)
        {
            DamageText damageText = Instantiate(damageTextPrefab, transform);

            damageText.Init(this, value);
        }

        public void OnTextFadeComplete(DamageText text)
        {

        }
    }
}
