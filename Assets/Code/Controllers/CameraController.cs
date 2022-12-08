using Code.Assistance;
using Code.Configs;
using Code.Interfaces;
using Code.Timer;
using UnityEngine;


namespace Code.Controllers
{
    public class CameraController : ILateExecute
    {
        private readonly CameraConfig _config;
        private readonly Transform _target;
        private readonly Transform _cameraTransform;
        public AudioSource AudioSource { get; }
        private ITimeRemaining _timeRemaining;
        private Vector3 _cameraOffset = Vector3.zero;
        private Transform _newTarget;
        private bool _isFollowing;


        public CameraController(Transform camera, CameraConfig config, Transform target)
        {
            _cameraTransform = camera.transform;
            _config = config;
            _target = target;
            AudioSource = camera.gameObject.GetOrAddComponent<AudioSource>();
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
            float currentAngle = _cameraTransform.eulerAngles.y;
            float desiredAngle = _target.eulerAngles.y;
            float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * _config.SmoothSpeed);

            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            var newPosition = _target.position + (rotation * _cameraOffset);
            _cameraTransform.position = newPosition;

            _cameraTransform.LookAt(_target.transform);
        }

        public void ChangeTarget(float time)
        {
            _isFollowing = false;
            _cameraTransform.position = _config.MainView.position;
            _cameraTransform.rotation = _config.MainView.rotation;
            _timeRemaining = new TimeRemaining(BackToTarget, time);
            _timeRemaining.AddTimeRemaining();
        }

        private void BackToTarget()
        {
            _isFollowing = true;
            _timeRemaining.RemoveTimeRemaining();
        }

        private void Cut()
        {
            _cameraOffset.z = -_config.Distance;
            _cameraOffset.y = _config.Height;
            _cameraTransform.position = _target.position + _target.TransformVector(_cameraOffset);
        }
    }
}
