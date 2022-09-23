using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public sealed class DamageTakenView : MonoBehaviour
    {
        [SerializeField]
        private Text text;

        private Action _onHide;
        
        public void ShowDamage(float value, Action onHide)
        {
            text.text = $"-{value.ToString(CultureInfo.InvariantCulture)}";
            _onHide = onHide;
            
            StartCoroutine(HideAfterSeconds());
        }

        private void Update()
        {
            transform.position += Vector3.one * 0.75f * Time.deltaTime;
        }

        private IEnumerator HideAfterSeconds()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            _onHide?.Invoke();
        }
    }
}