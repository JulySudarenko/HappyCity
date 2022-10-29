using System;
using System.Collections.Generic;
using Code.Configs;
using Code.Network;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.ResourcesSpawn
{
    internal class ResourcesSpawner : IDisposable
    {
        private readonly List<ResourceHandler> _resourcesList;
        private readonly List<int> _resourcesIDList = new List<int>();

        public ResourcesSpawner(Vector3[] placeGenerator, ResourcesConfig resourcesConfig,
            int characterID, NetworkSynchronizationController networkSynchronizer)
        {
            var resources = new List<Transform>();
            _resourcesList = new List<ResourceHandler>();

            for (int j = 0; j < placeGenerator.Length; j++)
            {
                var res = Object.Instantiate(resourcesConfig.Resource, placeGenerator[j],
                    resourcesConfig.Resource.rotation);
                resources.Add(res.transform);
                var resource = new ResourceHandler(res.transform, characterID, placeGenerator[j], networkSynchronizer);
                _resourcesList.Add(resource);
                _resourcesIDList.Add(res.gameObject.GetInstanceID());
            }

            Init();
        }

        public int[] ResourcesList => _resourcesIDList.ToArray();

        private void Init()
        {
            for (int i = 0; i < _resourcesList.Count; i++)
            {
                _resourcesList[i].Initialize();
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _resourcesList.Count; i++)
            {
                _resourcesList[i].Cleanup();
            }
        }
    }
}
