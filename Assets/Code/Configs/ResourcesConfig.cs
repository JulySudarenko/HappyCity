using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "ResourcesConfig", order = 0)]
    public sealed class ResourcesConfig : ScriptableObject
    {
        public Transform Wood;
        public Transform Stone;
        public Transform Food;
        
        [SerializeField] private int _woodCount = 10;
        [SerializeField] private int _stoneCount = 5;
        [SerializeField] private int _radius = 5;

        public int Radius => _radius;

        public int WoodCount => _woodCount;

        public int StoneCount => _stoneCount;
    }
}
