using Code.Interfaces;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code.Network
{
    internal class StartingQuestQueueDataSender : IInitialization, ICleanup
    {
        private readonly Vector3[] _questQueue;
        private readonly PhotonConnectionController _connectionController;

        public StartingQuestQueueDataSender(Vector3[] questQueue, PhotonConnectionController connectionController)
        {
            _questQueue = questQueue;
            _connectionController = connectionController;
        }

        public void Initialize()
        {
            _connectionController.NewPlayerConnection += SendQuestQueue;
        }

        private void SendQuestQueue()
        {
            PhotonNetwork.RaiseEvent(120, _questQueue.Length,
                new RaiseEventOptions() {Receivers = ReceiverGroup.All},
                new SendOptions() {Reliability = true});
            
            for (int i = 0; i < _questQueue.Length; i++)
            {
                PhotonNetwork.RaiseEvent(121, _questQueue[i],
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
