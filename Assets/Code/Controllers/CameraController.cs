using UnityEngine;

namespace Code.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _distance = 7.0f;
        [SerializeField] private float _height = 3.0f;
        [SerializeField] private Vector3 _centerOffset = Vector3.zero;
        [SerializeField] private bool _followOnStart = false;
        private float _smoothSpeed = 0.125f;
        private Transform _cameraTransform;
        private bool _isFollowing;
        private Vector3 _cameraOffset = Vector3.zero;

        #region MonoBehaviour Callbacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase
        /// </summary>
        void Start()
        {
            // Start following the target if wanted.
            if (_followOnStart)
            {
                OnStartFollowing();
            }
        }


        void LateUpdate()
        {
            // The transform target may not destroy on level load, 
            // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
            if (_cameraTransform == null && _isFollowing)
            {
                OnStartFollowing();
            }

            // only follow is explicitly declared
            if (_isFollowing)
            {
                Follow();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raises the start following event. 
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing()
        {
            _cameraTransform = Camera.main.transform;
            _isFollowing = true;
            // we don't smooth anything, we go straight to the right camera shot
            Cut();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        void Follow()
        {
            _cameraOffset.z = -_distance;
            _cameraOffset.y = _height;
            //_cameraTransform.position = this.transform.position + this.transform.TransformVector(_cameraOffset);
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                 this.transform.position + this.transform.TransformVector(_cameraOffset), _smoothSpeed * Time.deltaTime);

            _cameraTransform.LookAt(this.transform.position + _centerOffset);
        }


        void Cut()
        {
            _cameraOffset.z = -_distance;
            _cameraOffset.y = _height;

            _cameraTransform.position = this.transform.position + this.transform.TransformVector(_cameraOffset);

            _cameraTransform.LookAt(this.transform.position + _centerOffset);
        }

        #endregion
    }
}
