using Code.Assistance;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "UnionResourcesConfig", menuName = "UnionResourcesConfig", order = 0)]
    public sealed class UnionConfig : ScriptableObject
    {
        [SerializeField] private string _resourcesFolder = "ResConfigs";
        [SerializeField] private string _questFolder = "QuestConfigs";
        [SerializeField] private string _placeFolder = "PlacesConfigs";
        
        private ResourcesConfig[] _resourcesConfigs;
        private QuestNpcConfig[] _questNpcConfigs;
        private BuildingPlacesConfig[] _buildingPlacesConfigs;

        public ResourcesConfig[] AllResourcesConfigs
        {
            get
            {
                _resourcesConfigs = Assistant.LoadAll<ResourcesConfig>(_resourcesFolder);
                return _resourcesConfigs;
            }
        }
        
        public QuestNpcConfig[] AllQuestNpcConfigs
        {
            get
            {
                _questNpcConfigs = Assistant.LoadAll<QuestNpcConfig>(_questFolder);
                return _questNpcConfigs;
            }
        }
        
        public BuildingPlacesConfig[] AllBuildingPlacesConfigs
        {
            get
            {
                _buildingPlacesConfigs = Assistant.LoadAll<BuildingPlacesConfig>(_placeFolder);
                return _buildingPlacesConfigs;
            }
        }
    }
}
