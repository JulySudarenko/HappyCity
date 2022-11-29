using UnityEngine;


namespace Code.Configs
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Data/BuildingConfig", order = 0)]
    public sealed class BuildingConfig : ScriptableObject
    {
        [Header("Building")] 
        public Transform[] Prefab;
        public Transform[] Places;
        public BuildingType BuildingType;
        [SerializeField] private int _numberOfBuildings;

        [Header("Required resources")] 
        [SerializeField] private int _woodCost = 10;
        [SerializeField] private int _foodCost = 10;
        [SerializeField] private int _stoneCost = 10;
        [SerializeField] private int _goldCost = 10;

        [Header("Build reward")] 
        [SerializeField] private int _population = 3;
        [SerializeField] private int _goldReward = 3;

        public int WoodCost => _woodCost;
        public int FoodCost => _foodCost;
        public int StoneCost => _stoneCost;
        public int GoldCost => _goldCost;
        public int Population => _population;
        public int GoldReward => _goldReward;

        public int NumberOfBuildings => _numberOfBuildings;
    }

    public enum BuildingType
    {
        House = 0,
        Farm = 1,
        Store = 2,
        Road = 3,
        Hospital = 4,
        CityHall = 5
    }
}
