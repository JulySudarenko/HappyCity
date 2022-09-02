using System.Collections.Generic;
using Code.Configs;
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

        private void CreatePlaces(Vector3 center, ResourcesConfig config)
        {
            bool flag = false;
            while (_places.Count < config.WoodCount)
            {
                var newVector = GenerateVector(center, config.Radius);
                Debug.Log($"");
                foreach (var place in _places)
                {
                    // if (place == newVector)
                    // {
                    //     flag = true;
                    // }

                    var distance = (newVector.x - place.x) * (newVector.x - place.x) +
                                   (newVector.y - place.y) * (newVector.y - place.y);
                    if (distance < 9)
                    {
                        flag = true;
                        Debug.Log("Small distance!");
                    }
                }

                if (!flag)
                {
                    _places.Add(newVector);
                    Debug.Log(newVector);
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
