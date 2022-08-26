using System.Collections.Generic;
using System.Linq;
using Code.Configs;
using Code.View;
using Photon.Pun;
using UnityEngine;


namespace Code.Factory
{
    internal class ResourcesSpawnPlacesGenerator
    {
        private List<Vector3> _places;

        public ResourcesSpawnPlacesGenerator(Vector3 center, ResourcesConfig config)
        {
            _places = new List<Vector3>();
            CreatePlaces(center, config);
        }

        public Vector3[] SpawnPlaces => _places.ToArray();

        public List<Vector3> Places => _places;

        private void CreatePlaces(Vector3 center, ResourcesConfig config)
        {
            for (int i = 0; i < config.WoodCount; i++)
            {
                bool flag = false;
                var newVector = GenerateVector(center, config.Radius);
                foreach (var place in _places.Where(place => place == newVector))
                {
                    flag = true;
                }

                if (!flag)
                {
                    _places.Add(newVector);
                    Debug.Log(newVector);
                }
            }
        }

        private Vector3 GenerateVector(Vector3 center, float radius)
        {
            var newPlaceX = Random.Range(center.x - radius, center.x + radius);
            var newPlaceZ = Random.Range(center.z - radius, center.z + radius);

            var vector = new Vector3(newPlaceX, 1.0f, newPlaceZ);
            return vector;
        }
    }

    internal class ResourcesSpawnHandler
    {
        private readonly List<Transform> _woods;
        private readonly List<Vector3> _woodsPlaces;
        private readonly List<Transform> _stones;

        public ResourcesSpawnHandler(SpawnPlacesView spawnPlacesView, ResourcesConfig resourcesConfig)
        {
            _woods = new List<Transform>();
            _woodsPlaces = new List<Vector3>();
            
            for (int i = 0; i < spawnPlacesView.ForestPlaces.Length; i++)
            {
                var placeGenerator =
                    new ResourcesSpawnPlacesGenerator(spawnPlacesView.ForestPlaces[i].position, resourcesConfig);
                _woodsPlaces = placeGenerator.Places;

                for (int j = 0; j < _woodsPlaces.Count; j++)
                {
                     Debug.Log(_woodsPlaces[i]);
                }
                
                for (int j = 0; j < placeGenerator.Places.Count; j++)
                {
                    Debug.Log(placeGenerator.Places[i]);
                }

                // for (int j = 0; j < placeGenerator.Places.Count; j++)
                // {
                //     Debug.Log(placeGenerator.Places[i]);
                //     var number = PhotonNetwork.LocalPlayer.ActorNumber;
                //     var wood = PhotonNetwork.Instantiate(resourcesConfig.Wood.gameObject.name,
                //         placeGenerator.Places[i], Quaternion.Euler(0.0f, 0.0f, 45.0f));
                //     _woods.Add(wood.transform);
                // }


                // for (int j = 0; j < placeGenerator.SpawnPlaces.Length; j++)
                // {
                //     var wood = PhotonNetwork.Instantiate(resourcesConfig.Wood.gameObject.name,
                //         placeGenerator.SpawnPlaces[i], Quaternion.Euler(0, 0, 30));
                //     _woods.Add(wood.transform);
                //     Debug.Log(placeGenerator.SpawnPlaces[i]);
                // }
            }
        }
        
        
        // private void CreatePlaces(Vector3 center, ResourcesConfig config)
        // {
        //     for (int i = 0; i < config.WoodCount; i++)
        //     {
        //         bool flag = false;
        //         var newVector = GenerateVector(center, config.Radius);
        //         foreach (var place in _places.Where(place => place == newVector))
        //         {
        //             flag = true;
        //         }
        //
        //         if (!flag)
        //         {
        //             _places.Add(newVector);
        //             //Debug.Log(newVector);
        //         }
        //     }
        // }
        //
        //
        // private Vector3 GenerateVector(Vector3 center, float radius)
        // {
        //     var newPlaceX = Random.Range(center.x - radius, center.x + radius);
        //     var newPlaceZ = Random.Range(center.z - radius, center.z + radius);
        //
        //     var vector = new Vector3(newPlaceX, 1.0f, newPlaceZ);
        //     return vector;
        // }
    }
}
