using Code.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public sealed class HpBarView : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private Image hpBarImage;

        [SerializeField]
        private Text hpBarText;

        private Camera _camera;
        
        private void Awake()
        {
            _camera = Camera.main;
            canvas.worldCamera = _camera;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(_camera.transform.position * -1);
        }

        public void SetValue(Parameter hpParameter)
        {
            hpBarText.text = hpParameter.Value.ToString("F");
            hpBarImage.fillAmount = hpParameter.Value / hpParameter.InitValue;
        }
    }
}