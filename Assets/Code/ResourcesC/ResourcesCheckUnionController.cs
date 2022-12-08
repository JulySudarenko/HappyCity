using System;
using Code.ResourcesSpawn;


namespace Code.ResourcesC
{
    internal class ResourcesCheckUnionController
    {
        private readonly ResourceCounterController _woodCounter;
        private readonly ResourceCounterController _foodCounter;
        private readonly ResourceCounterController _stoneCounter;
        private readonly ResourceCounterController _goldCounter;
        private readonly ResourceCounterController _happyCounter;

        public ResourcesCheckUnionController(ResourceCounterController woodCounter, ResourceCounterController foodCounter,
            ResourceCounterController stoneCounter, ResourceCounterController goldCounter, ResourceCounterController happyCounter)
        {
            _woodCounter = woodCounter;
            _foodCounter = foodCounter;
            _stoneCounter = stoneCounter;
            _goldCounter = goldCounter;
            _happyCounter = happyCounter;
        }

        public ResourceCounterController WoodCounter => _woodCounter;
        public ResourceCounterController FoodCounter => _foodCounter;
        public ResourceCounterController StoneCounter => _stoneCounter;
        public ResourceCounterController GoldCounter => _goldCounter;
        public ResourceCounterController HappyCounter => _happyCounter ;
        
        
        public bool CheckResources(ResourcesType type, int count)
        {
            bool flag;

            switch (type)
            {
                case ResourcesType.Wood:
                    flag = Check(_woodCounter, count);
                    break;
                case ResourcesType.Stone:
                    flag = Check(_stoneCounter, count);
                    break;
                case ResourcesType.Food:
                    flag = Check(_foodCounter, count);
                    break;
                case ResourcesType.Gold:
                    flag = Check(_woodCounter, count);
                    break;                
                case ResourcesType.Happiness:
                    flag = Check(_happyCounter, count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return flag;
        }

        private bool Check(ResourceCounterController counterController, int count)
        {
            if (count <= counterController.Count)
            {
                return true;
            }

            return false;
        }

        public void GrandResources(ResourcesType type, int count)
        {
            switch (type)
            {
                case ResourcesType.Wood:
                    _woodCounter.OnGrandResource(count);
                    break;
                case ResourcesType.Stone:
                    _stoneCounter.OnGrandResource(count);
                    break;
                case ResourcesType.Food:
                    _foodCounter.OnGrandResource(count);
                    break;
                case ResourcesType.Gold:
                    _goldCounter.OnGrandResource(count);
                    break;                
                case ResourcesType.Happiness:
                    _happyCounter.OnGrandResource(count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void SpendResources(ResourcesType type, int count)
        {
            switch (type)
            {
                case ResourcesType.Wood:
                    _woodCounter.OnSpendResource(count);
                    break;
                case ResourcesType.Stone:
                    _stoneCounter.OnSpendResource(count);
                    break;
                case ResourcesType.Food:
                    _foodCounter.OnSpendResource(count);
                    break;
                case ResourcesType.Gold:
                    _goldCounter.OnSpendResource(count);
                    break;                
                case ResourcesType.Happiness:
                    _happyCounter.OnSpendResource(count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
