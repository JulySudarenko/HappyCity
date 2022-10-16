using System;
using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.ResourcesSpawn;


namespace Code.Controllers
{
    internal class ResourceCounterController : IInitialization, ICleanup
    {
        public Action<int> ChangeCount;
        private readonly ResourcesKeeper _resourcesKeeper;
        private readonly ResourcesConfig _config;
        private ResourcesSpawner _resourcesSpawner;
        private readonly CharacterModel _character;

        public ResourceCounterController(ResourcesConfig config, CharacterModel character, ResourcesType type)
        {
            _config = config;
            _character = character;
            _resourcesKeeper = new ResourcesKeeper(_config.StartValue, type);
        }

        public int Count => _resourcesKeeper.ResourceCount;
        public ResourcesType Type => _resourcesKeeper.Type;

        public void Init(ResourcesSpawner resourcesSpawner)
        {
            _resourcesSpawner = resourcesSpawner;
        }

        private void OnPickUpResource(int resID, int characterID)
        {
            var resList = _resourcesSpawner.ResourcesList;
            for (int i = 0; i < resList.Length; i++)
            {
                if (resList[i] == resID)
                {
                    _resourcesKeeper.Add(_config.MiningCount);
                    ChangeCount?.Invoke(_resourcesKeeper.ResourceCount);
                }
            }
        }

        public void OnGrandResource(int count)
        {
            _resourcesKeeper.Add(count);
            ChangeCount?.Invoke(_resourcesKeeper.ResourceCount);
        }

        public void OnSpendResource(int count)
        {
            _resourcesKeeper.Remove(count);
            ChangeCount?.Invoke(_resourcesKeeper.ResourceCount);
        }

        public void Initialize()
        {
            _character.HitHandler.OnHitEnter += OnPickUpResource;
        }

        public void Cleanup()
        {
            _character.HitHandler.OnHitEnter -= OnPickUpResource;
        }
    }

    internal class ResourcesKeeper
    {
        private int _resourceCount;
        private readonly ResourcesType _type;

        public ResourcesKeeper(int resourceCount, ResourcesType type)
        {
            _resourceCount = resourceCount;
            _type = type;
        }

        public int ResourceCount => _resourceCount;

        public ResourcesType Type => _type;

        public void Add(int count)
        {
            _resourceCount += count;
        }

        public void Remove(int count)
        {
            _resourceCount -= count;
        }
    }
}
