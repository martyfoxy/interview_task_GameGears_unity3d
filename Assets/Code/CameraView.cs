using System.Collections;
using Code.Data;
using Code.Settings;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Code
{
    public sealed class CameraView : MonoBehaviour, IInitializable, ITickable
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private float speed = 30f;

        [SerializeField]
        private float zoomSpeed = 50f;

        private SettingsRepository _settingsRepository;
        private CameraModel _cameraSettings;
        private bool _isZoomed;
        private float _randOffset;

        [Inject]
        public void Inject(SettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }
        
        public void Initialize()
        {
            _cameraSettings = _settingsRepository.Settings.cameraSettings;
            camera.fieldOfView = _cameraSettings.fovMax;

            StartCoroutine(WaitRandomDoAction());
        }
        
        public void Tick()
        {
            transform.Rotate(0f, speed * Time.deltaTime, 0f);

            if (_isZoomed)
                ZoomOut();
            else
                ZoomIn();
        }

        private IEnumerator WaitRandomDoAction()
        {
            while (true)
            {
                var randTime = Random.Range(1, _cameraSettings.fovDelay);
                yield return new WaitForSeconds(randTime);

                _isZoomed = !_isZoomed;
                _randOffset = Random.Range(0, 20f);
                
                randTime = Random.Range(1, _cameraSettings.fovDuration);
                yield return new WaitForSeconds(randTime);
            }
        }
        
        private void ZoomIn()
        {
            camera.fieldOfView -= zoomSpeed * Time.deltaTime;
            camera.fieldOfView = Mathf.Max(_cameraSettings.fovMin + _randOffset, camera.fieldOfView);
        }
        
        private void ZoomOut()
        {
            camera.fieldOfView += zoomSpeed * Time.deltaTime;
            camera.fieldOfView = Mathf.Min(_cameraSettings.fovMax, camera.fieldOfView);
        }
    }
}
