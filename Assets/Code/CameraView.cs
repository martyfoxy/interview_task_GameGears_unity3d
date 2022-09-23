using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public sealed class CameraView : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;

        [SerializeField]
        private float speed = 30f;

        [SerializeField]
        private float zoomSpeed = 50f;

        private float _defaultFov;
        private float _minFov;
        private bool _isZoomed;
        private float _randOffset;

        private void Start()
        {
            _defaultFov = camera.fieldOfView;
            _minFov = _defaultFov / 2;

            StartCoroutine(WaitRandomDoAction());
        }

        private void Update()
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
                var randTime = Random.Range(1, 5);
                yield return new WaitForSeconds(randTime);

                _isZoomed = !_isZoomed;
                _randOffset = Random.Range(0, 20f);
                
                randTime = Random.Range(1, 5);
                yield return new WaitForSeconds(randTime);
            }
        }
        
        private void ZoomIn()
        {
            camera.fieldOfView -= zoomSpeed * Time.deltaTime;
            camera.fieldOfView = Mathf.Max(_minFov + _randOffset, camera.fieldOfView);
        }
        
        private void ZoomOut()
        {
            camera.fieldOfView += zoomSpeed * Time.deltaTime;
            camera.fieldOfView = Mathf.Min(_defaultFov, camera.fieldOfView);
        }
    }
}
