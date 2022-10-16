using System.Collections.Generic;
using Code.Configs;
using UnityEngine;

namespace Code.ResourcesSpawn
{
    internal class ResourcesSpawnPlacesGenerator
    {
        private readonly List<Vector3> _places;
        private const int MAX_COUNT = 10;

        public ResourcesSpawnPlacesGenerator(Vector3 center, ResourcesConfig config)
        {
            _places = new List<Vector3>();
            CreatePlaces(center, config);
        }

        public Vector3[] SpawnPlaces => _places.ToArray();

        private void CreatePlaces(Vector3 center, ResourcesConfig config)
        {
            bool flag = false;
            int actionsCounter = 0;
            while (_places.Count < config.SpawnCount)
            {
                var newVector = GenerateVector(center, config.Radius);
                foreach (var place in _places)
                {
                    var distance = (newVector.x - place.x) * (newVector.x - place.x) +
                                   (newVector.y - place.y) * (newVector.y - place.y);
                    if (distance < config.Distance * config.Distance)
                    {
                        flag = true;
                        actionsCounter++;
                        if (actionsCounter == MAX_COUNT)
                        {
                            flag = false;
                            actionsCounter = 0;
                        }
                    }
                }

                if (!flag)
                {
                    _places.Add(newVector);
                }

                flag = false;
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
}
