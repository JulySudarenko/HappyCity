using Code.Factory;
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
        [SerializeField] private int _count = 10;
        [SerializeField] private int _radius = 25;
        [SerializeField] private int _distance = 3;
        [SerializeField] private int _startValue = 10;

        public float MiningTime => _miningTime;

        public int Radius => _radius;

        public int Count => _count;

        public int Distance => _distance;

        public int StartValue => _startValue;
    }
}
