using Code.Assistance;
using Code.Hit;
using Code.Interfaces;
using Code.Network;
using Code.Timer;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Code.ResourcesSpawn
{
    internal class ResourceHandler : IInitialization, ICleanup
    {
        private readonly NetworkSynchronizationController _networkSynchronizer;
        private readonly Transform _resTransform;
        private readonly HitHandler _resHit;
        private readonly Vector3 _startPlace;
        private TimeRemaining _timeRemaining;
        private readonly int _characterID;


        public ResourceHandler(Transform resTransform, int characterID, Vector3 startPlace,
            NetworkSynchronizationController networkSynchronizer)
        {
            _resTransform = resTransform;
            _characterID = characterID;
            _startPlace = startPlace;
            _networkSynchronizer = networkSynchronizer;
            _resHit = resTransform.gameObject.GetOrAddComponent<HitHandler>();
        }

        public void Initialize()
        {
            _resHit.OnHitEnter += OnSelfGetResources;
            _networkSynchronizer.RemoveResource += OnOtherGetResources;
            _networkSynchronizer.InstallResource += OnOtherSpawnResources;
        }

        private void OnOtherGetResources(Vector3 startPosition)
        {
            if (startPosition == _startPlace)
            {
                _resTransform.gameObject.SetActive(false);
            }
        }

        private void OnOtherSpawnResources(Vector3 startPosition)
        {
            if (startPosition == _startPlace)
            {
                _resTransform.gameObject.SetActive(true);
            }
        }

        private void OnSelfGetResources(int contactID, int resID)
        {
            if (contactID == _characterID)
            {
                _resTransform.gameObject.SetActive(false);
                _timeRemaining = new TimeRemaining(Reinstall, Random.Range(5.0f, 10.0f));
                _timeRemaining.AddTimeRemaining();

                PhotonNetwork.RaiseEvent(114, _startPlace,
                    new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                    new SendOptions() {Reliability = true});
            }
        }

        private void Reinstall()
        {
            PhotonNetwork.RaiseEvent(115, _startPlace,
                new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                new SendOptions() {Reliability = true});

            _resTransform.gameObject.SetActive(true);
            _timeRemaining.RemoveTimeRemaining();
        }

        public void Cleanup()
        {
            _resHit.OnHitEnter -= OnSelfGetResources;
            _networkSynchronizer.RemoveResource -= OnOtherGetResources;
            _networkSynchronizer.InstallResource -= OnOtherSpawnResources;
        }
    }
}
