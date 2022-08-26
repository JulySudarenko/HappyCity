using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Data/PlayerConfig", order = 0)]
    public sealed class PlayerConfig : ScriptableObject
    {
        public Transform FemalePrefab;
        public Transform MalePrefab;
        public Transform SpawnPoints;

        [SerializeField] private int _happiness = 100;
        [SerializeField] private float _speed = 10.0f;

        public int Happiness => _happiness;

        public float Speed => _speed;
    }
}
