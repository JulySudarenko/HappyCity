using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "BuildingPlacesConfig", menuName = "Data/BuildingPlacesConfig", order = 0)]
    public sealed class BuildingPlacesConfig : ScriptableObject
    {
        public Transform[] Places;
        [SerializeField] private int _stage;

        public int Stage => _stage;
    }
}
