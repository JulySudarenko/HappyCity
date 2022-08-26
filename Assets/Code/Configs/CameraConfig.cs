using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Data/CameraSettings", order = 0)]
    public sealed class CameraConfig : ScriptableObject
    {
        [SerializeField] private float _distance = 7.0f;
        [SerializeField] private float _height = 3.0f;
        [SerializeField] private Vector3 _centerOffset = Vector3.zero;
        [SerializeField] private bool _followOnStart = false;
        [SerializeField] private float _smoothSpeed = 0.125f;

        public Vector3 CenterOffset => _centerOffset;

        public float Distance => _distance;

        public float Height => _height;

        public float SmoothSpeed => _smoothSpeed;

        public bool FollowOnStart => _followOnStart;
    }
}
