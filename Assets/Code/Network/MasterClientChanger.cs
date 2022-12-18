using Code.Interfaces;
using Code.ResourcesSpawn;
using Photon.Pun;


namespace Code.Network
{
    internal class MasterClientChanger : ICleanup
    {
        private readonly PhotonConnectionController _connectionController;
        private readonly NetworkSynchronizationController _networkSynchronizationController;
        private StartingResourcesDataSender _startingResourcesDataSender;
        private StartingQuestQueueDataSender _questQueueDataSender;

        public MasterClientChanger(PhotonConnectionController connectionController,
            NetworkSynchronizationController networkSynchronizationController)
        {
            _connectionController = connectionController;
            _networkSynchronizationController = networkSynchronizationController;

            _connectionController.NewMasterConnection += OnChangeMaster;
        }

        private void OnChangeMaster()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var generator = new ResourcesPlaceGeneratorLists(_networkSynchronizationController);
                _startingResourcesDataSender = new StartingResourcesDataSender(generator, _connectionController);
                _questQueueDataSender = new StartingQuestQueueDataSender(_connectionController,
                    _networkSynchronizationController);
                _startingResourcesDataSender.Initialize();
            }
        }

        public void Cleanup()
        {
            if (_connectionController.NewMasterConnection != null)
                _connectionController.NewMasterConnection -= OnChangeMaster;
            if (_startingResourcesDataSender != null) _startingResourcesDataSender.Cleanup();
            if (_questQueueDataSender != null) _questQueueDataSender.Cleanup();
        }
    }
}
