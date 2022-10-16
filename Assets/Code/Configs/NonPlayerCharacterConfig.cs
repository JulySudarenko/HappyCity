using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "NonPlayerCharacterConfig", menuName = "Data/NonPlayerCharacterConfig", order = 0)]
    public sealed class NonPlayerCharacterConfig : ScriptableObject
    {
        public Transform Prefab;
        public Transform SpawnPoints;

        [SerializeField] private int _happiness = 100;
        [SerializeField] private float _speed = 10.0f;

        public int Happiness => _happiness;

        public float Speed => _speed;
    }
}
