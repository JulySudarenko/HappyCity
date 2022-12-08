using Code.Interfaces;
using Code.ResourcesSpawn;
using Photon.Pun;
using UnityEngine;

namespace Code.Network
{
    internal class MasterClientChanger : ICleanup
    {
        private readonly PhotonConnectionController _connectionController;
        private readonly NetworkSynchronizationController _networkSynchronizationController;
        private StartingResourcesDataSender _startingResourcesDataSender;
        private StartingQuestQueueDataSender _questQueueDataSender;

        public MasterClientChanger(PhotonConnectionController connectionController, NetworkSynchronizationController networkSynchronizationController)
        {
            _connectionController = connectionController;
            _networkSynchronizationController = networkSynchronizationController;

            _connectionController.NewMasterConnection += OnChangeMaster;
        }

        private void OnChangeMaster()
        {
            Debug.Log("Change");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("New master changes");
                var generator = new ResourcesPlaceGeneratorLists(_networkSynchronizationController);
                _startingResourcesDataSender =  new StartingResourcesDataSender(generator, _connectionController);
                _questQueueDataSender = new StartingQuestQueueDataSender(_networkSynchronizationController.QuestFirstQueue.ToArray(), _connectionController);
                _startingResourcesDataSender.Initialize();
                _questQueueDataSender.Initialize();
            }
        }

        public void Cleanup()
        {
            _connectionController.NewMasterConnection -= OnChangeMaster;
            _startingResourcesDataSender.Cleanup();
            _questQueueDataSender.Cleanup();
            
        }
    }
}
