using UnityEngine;

namespace Code.View
{
    public class SpawnPlacesView : MonoBehaviour
    {
        [SerializeField] private Transform[] _forestPlaces;
        [SerializeField] private Transform[] _stonePlaces;

        public Transform[] ForestPlaces => _forestPlaces;

        public Transform[] StonePlaces => _stonePlaces;
    }
}
