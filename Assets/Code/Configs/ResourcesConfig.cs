using Code.Factory;
using Code.ResourcesSpawn;
using UnityEngine;


namespace Code.Configs
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "ResourcesConfig", order = 0)]
    public sealed class ResourcesConfig : ScriptableObject
    {
        public Transform Resource;
        public Sprite Icon;
        public ResourcesType Type;
        [SerializeField] private float _miningTime = 3.0f;
        [SerializeField] private int _spawnCount = 10;
        [SerializeField] private int _radius = 25;
        [SerializeField] private int _distance = 3;
        [SerializeField] private int _startValue = 10;
        [SerializeField] private int _miningCount = 5;

        public float MiningTime => _miningTime;

        public int Radius => _radius;

        public int SpawnCount => _spawnCount;

        public int Distance => _distance;

        public int StartValue => _startValue;

        public int MiningCount => _miningCount;
    }
}
