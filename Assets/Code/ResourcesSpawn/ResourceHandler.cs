using Code.Assistance;
using Code.Factory;
using Code.Interfaces;
using Code.Timer;
using UnityEngine;

namespace Code.ResourcesSpawn
{
    internal class ResourceHandler : IInitialization, ICleanup
    {
        private readonly Transform _resTransform;
        private readonly HitHandler _resHit;
        private TimeRemaining _timeRemaining;
        private readonly int _characterID;
        
        public ResourceHandler(Transform resTransform, int characterID)
        {
            _resTransform = resTransform;
            _characterID = characterID;
            _resHit = resTransform.gameObject.GetOrAddComponent<HitHandler>();
        }
        
        public void Initialize()
        {
            _resHit.IsHit += OnGetResources;
        }

        private void OnGetResources(int contactID, string tag)
        {
            if (contactID == _characterID)
            {
                _resTransform.gameObject.SetActive(false);
                _timeRemaining = new TimeRemaining(Reinstall, Random.Range(5.0f, 10.0f));
                _timeRemaining.AddTimeRemaining();
            }
        }

        private void Reinstall()
        {
            _resTransform.gameObject.SetActive(true);
            _timeRemaining.RemoveTimeRemaining();
        }
        
        public void Cleanup()
        {
            _resHit.IsHit -= OnGetResources;
        }
    }
}
