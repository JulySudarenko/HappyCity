using System;
using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.ResourcesSpawn;

namespace Code.ResourcesC
{
    internal class ResourceCounterController : ICleanup
    {
        public Action<int> ChangeCount;
        private readonly IKeeper _resourcesKeeper;
        private readonly ResourcesConfig _config;
        private ResourcesSpawner _resourcesSpawner;
        private readonly CharacterModel _character;
        private int[] _resList;

        public ResourceCounterController(ResourcesConfig config, CharacterModel character, ResourcesType type)
        {
            _config = config;
            _character = character;
            _resourcesKeeper = new ResourcesKeeper(_config.StartValue, type);
        }

        public int Count => _resourcesKeeper.ResourceCount();
        public ResourcesType Type => _resourcesKeeper.Type();

        public void Init(ResourcesSpawner resourcesSpawner)
        {
            _resourcesSpawner = resourcesSpawner;
            _resList = _resourcesSpawner.ResourcesList;
            _character.HitHandler.OnHitEnter += OnPickUpResource;
        }

        private void OnPickUpResource(int resID, int characterID)
        {
            if(_resList.Length > 0)
            {
                for (int i = 0; i < _resList.Length; i++)
                {
                    if (_resList[i] == resID)
                    {
                        _resourcesKeeper.Add(_config.MiningCount);
                        ChangeCount?.Invoke(_resourcesKeeper.ResourceCount());
                    }
                }
            }
        }

        public void OnGrandResource(int count)
        {
            _resourcesKeeper.Add(count);
            ChangeCount?.Invoke(_resourcesKeeper.ResourceCount());
        }

        public void OnSpendResource(int count)
        {
            _resourcesKeeper.Remove(count);
            ChangeCount?.Invoke(_resourcesKeeper.ResourceCount());
        }

        public void Cleanup()
        {
            _character.HitHandler.OnHitEnter -= OnPickUpResource;
        }
    }
}
