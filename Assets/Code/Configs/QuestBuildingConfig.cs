using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "QuestBuildingConfig", menuName = "Data/QuestBuildingConfig", order = 0)]
    public sealed class QuestBuildingConfig : QuestConfig
    {
        [SerializeField] private int _timeForChangeHappiness;
        [SerializeField] private int _happinessRemove;
        
        [SerializeField] private string _buildingConfigPath = "HomeBuildingConfig";
        
        private BuildingConfig _buildingConfig;
        
        public BuildingConfig BuildingConfig
        {
            get
            {
                if (_buildingConfig == null)
                {
                    _buildingConfig = Assistance.Assistant.Load<BuildingConfig>(_buildingConfigPath);
                }

                return _buildingConfig;
            }
        }

        public int TimeForChangeHappiness => _timeForChangeHappiness;

        public int HappinessRemove => _happinessRemove;
    }
}
