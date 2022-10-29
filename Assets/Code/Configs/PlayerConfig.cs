using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Data/PlayerConfig", order = 0)]
    public sealed class PlayerConfig : ScriptableObject
    {
        public Transform FemalePrefab;
        public Transform MalePrefab;
        public Transform SpawnPoints;
        public Transform PlayerView;

        [SerializeField] private float _speed = 10.0f;
        [SerializeField] private float _height = 1.7f;


        public float Speed => _speed;

        public float Height => _height;
    }
}
