using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "NonPlayerCharacterConfig", menuName = "Data/NonPlayerCharacterConfig", order = 0)]
    public sealed class NonPlayerCharacterConfig : ScriptableObject
    {
        public Transform Prefab;
        public Transform SpawnPoints;
        public Transform NpcView;

        [SerializeField] private int _happiness = 100;
        [SerializeField] private float _speed = 10.0f;
        [SerializeField] private float _height = 1.7f;


        public float Speed => _speed;
        public float Height => _height;
        public int Happiness => _happiness;
    }
}
