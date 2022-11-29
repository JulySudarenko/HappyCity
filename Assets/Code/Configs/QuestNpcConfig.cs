using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "QuestNpcConfig", menuName = "Data/QuestNpcConfig", order = 0)]
    public sealed class QuestNpcConfig : QuestConfig
    {
        [SerializeField] private string _npcConfigPath = "NonPlayerCharacterConfig";
        [SerializeField] private string _buildingConfigPath = "HomeBuildingConfig";


        private NonPlayerCharacterConfig _npcConfig;
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
        
        public NonPlayerCharacterConfig NpcConfig
        {
            get
            {
                if (_npcConfig == null)
                {
                    _npcConfig = Assistance.Assistant.Load<NonPlayerCharacterConfig>(_npcConfigPath);
                }

                return _npcConfig;
            }
        }
    }
}
