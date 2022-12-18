using Code.Interfaces;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code.Network
{
    internal class StartingQuestQueueDataSender : ICleanup
    {
        private readonly PhotonConnectionController _connectionController;
        private readonly NetworkSynchronizationController _networkSynchronizationController;

        public StartingQuestQueueDataSender(PhotonConnectionController connectionController,
            NetworkSynchronizationController networkSynchronizationController)
        {
            _connectionController = connectionController;
            _networkSynchronizationController = networkSynchronizationController;
            _connectionController.NewPlayerConnection += SendQuestQueue;
        }

        private void SendQuestQueue()
        {
            PhotonNetwork.RaiseEvent(120, _networkSynchronizationController.AllQuestsInfos.Count,
                new RaiseEventOptions() {Receivers = ReceiverGroup.All},
                new SendOptions() {Reliability = true});

            for (int i = 0; i < _networkSynchronizationController.AllQuestsInfos.Count; i++)
            {
                string questData = JsonUtility.ToJson(new NetQuestsInfo(
                    _networkSynchronizationController.AllQuestsInfos[i].Quest,
                    _networkSynchronizationController.AllQuestsInfos[i].State));


                PhotonNetwork.RaiseEvent(125, questData,
                    new RaiseEventOptions() {Receivers = ReceiverGroup.All},
                    new SendOptions() {Reliability = true});
            }
        }

        public void Cleanup()
        {
            _connectionController.NewPlayerConnection -= SendQuestQueue;
        }
    }
}
