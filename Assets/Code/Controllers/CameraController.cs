using Code.Configs;
using Code.Interfaces;
using UnityEngine;


namespace Code.Controllers
{
    public class CameraController : ILateExecute
    {
        private readonly CameraConfig _config;
        private readonly Transform _target;
        private readonly Transform _cameraTransform;
        private readonly bool _isFollowing;
        private Vector3 _cameraOffset = Vector3.zero;

        public CameraController(Transform camera, CameraConfig config, Transform target)
        {
            _cameraTransform = camera.transform;
            _config = config;
            _target = target;

            _isFollowing = true;
            Cut();
        }
        
        public void LateExecute(float deltaTime)
        {
            if (_isFollowing)
            {
                Follow();
            }
        }

        private void Follow()
        {
            _cameraOffset.z = -_config.Distance;
            _cameraOffset.y = _config.Height;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                _target.position + _target.TransformVector(_cameraOffset), _config.SmoothSpeed * Time.deltaTime);
            _cameraTransform.LookAt(_target.position + _config.CenterOffset);
        }

        private void Cut()
        {
            _cameraOffset.z = -_config.Distance;
            _cameraOffset.y = _config.Height;
            _cameraTransform.position = _target.position + _target.TransformVector(_cameraOffset);
            _cameraTransform.LookAt(_target.position + _config.CenterOffset);
        }
    }
}
