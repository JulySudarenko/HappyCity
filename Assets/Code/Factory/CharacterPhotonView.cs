using Code.Network;
using Photon.Pun;
using UnityEngine;

namespace Code.Factory
{
    public class CharacterPhotonView : MonoBehaviourPun
    {
        private NetworkSynchronizationController _networkSynchronizationController;
        [HideInInspector] public CharacterModel Model;
        public bool CheckIsMine()
        {
            return photonView.IsMine;
        }

        public void Init(NetworkSynchronizationController networkSynchronizationController, CharacterModel model)
        {
            _networkSynchronizationController = networkSynchronizationController;
            Model = model;
        }

        public void Start()
        {
            _networkSynchronizationController.AddPlayer(this);
        }
    }
}
