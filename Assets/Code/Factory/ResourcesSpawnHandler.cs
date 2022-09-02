using System.Collections.Generic;
using Code.Configs;
using Code.Interfaces;
using Photon.Pun;
using UnityEngine;

namespace Code.Factory
{
    internal class ResourcesSpawnHandler : IInitialization, ICleanup
    {
        private readonly List<Transform> _woods;
        private readonly List<Transform> _stones;
        private List<ResourceHandler> _resources;

        public ResourcesSpawnHandler(ResourcesSpawnPlacesGenerator placeGenerator, ResourcesConfig resourcesConfig,
            int characterID)
        {
            _woods = new List<Transform>();
            _resources = new List<ResourceHandler>();

            for (int j = 0; j < placeGenerator.SpawnPlaces.Length; j++)
            {
                var wood = PhotonNetwork.Instantiate(resourcesConfig.Wood.gameObject.name,
                    placeGenerator.SpawnPlaces[j], Quaternion.Euler(0, 0, 30));
                _woods.Add(wood.transform);
                var resource = new ResourceHandler(wood.transform, characterID, placeGenerator);
                _resources.Add(resource);
                Debug.Log(placeGenerator.SpawnPlaces[j]);
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < _resources.Count; i++)
            {
                _resources[i].Initialize();
            }
        }

        public void Cleanup()
        {
            for (int i = 0; i < _resources.Count; i++)
            {
                _resources[i].Cleanup();
            }
        }
    }
}
