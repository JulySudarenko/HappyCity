using System;
using System.Collections.Generic;
using Code.Configs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.ResourcesSpawn
{
    internal class ResourcesSpawner : IDisposable
    {
        private readonly List<ResourceHandler> _resources;

        public ResourcesSpawner(Vector3[] placeGenerator, ResourcesConfig resourcesConfig,
            int characterID)
        {
            var resources = new List<Transform>();
            _resources = new List<ResourceHandler>();

            for (int j = 0; j < placeGenerator.Length; j++)
            {
                var res = Object.Instantiate(resourcesConfig.Resource, placeGenerator[j],
                    resourcesConfig.Resource.rotation);
                resources.Add(res.transform);
                var resource = new ResourceHandler(res.transform, characterID);
                _resources.Add(resource);
            }
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _resources.Count; i++)
            {
                _resources[i].Initialize();
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _resources.Count; i++)
            {
                _resources[i].Cleanup();
            }
        }
    }
}
