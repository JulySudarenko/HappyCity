using Code.Assistance;
using Code.Interfaces;
using UnityEngine;

namespace Code.Factory
{
    internal class ResourceHandler : IInitialization, ICleanup
    {
        private readonly Transform _resTransform;
        private readonly HitHandler _resHit;
        private readonly ResourcesSpawnPlacesGenerator _placeGenerator;
        private readonly int _characterID;
        
        public ResourceHandler(Transform resTransform, int characterID, ResourcesSpawnPlacesGenerator placeGenerator)
        {
            _resTransform = resTransform;
            _characterID = characterID;
            _placeGenerator = placeGenerator;
            _resHit = resTransform.gameObject.GetOrAddComponent<HitHandler>();
        }


        public void Initialize()
        {
            _resHit.IsHit += OnGetResources;
        }

        private void OnGetResources(int contactID, int id)
        {
            if (contactID == _characterID)
            {
                _resTransform.gameObject.SetActive(false);
            }
        }

        private void GenerateNewPlace()
        {
            
        }
        
        public void Cleanup()
        {
            _resHit.IsHit -= OnGetResources;
        }
    }
}
