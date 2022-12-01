using Code.Interfaces;
using Code.ResourcesSpawn;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Code.Network
{
    internal class StartingResourcesDataSender : IInitialization, ICleanup
    {
        private readonly ResourcesPlaceGeneratorLists _placeGeneratorLists;
        private readonly PhotonConnectionController _connectionController;

        public StartingResourcesDataSender(ResourcesPlaceGeneratorLists placeGeneratorLists,
            PhotonConnectionController connectionController)
        {
            _placeGeneratorLists = placeGeneratorLists;
            _connectionController = connectionController;
        }

        public void Initialize()
        {
            _connectionController.NewPlayerConnection += SendResourcePlaces;
        }

        private void SendResourcePlaces()
        {
            for (int i = 0; i < _placeGeneratorLists.AllFoodPlaces.Length; i++)
            {
                PhotonNetwork.RaiseEvent(111, _placeGeneratorLists.AllFoodPlaces[i],
                    new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                    new SendOptions() {Reliability = true});
            }

            for (int i = 0; i < _placeGeneratorLists.AllWoodPlaces.Length; i++)
            {
                PhotonNetwork.RaiseEvent(112,
                    _placeGeneratorLists.AllWoodPlaces[i],
                    new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                    new SendOptions() {Reliability = true});
            }

            for (int i = 0; i < _placeGeneratorLists.AllStonePlaces.Length; i++)
            {
                PhotonNetwork.RaiseEvent(113,
                    _placeGeneratorLists.AllStonePlaces[i],
                    new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                    new SendOptions() {Reliability = true});
            }
        }

        public void Cleanup()
        {
            _connectionController.NewPlayerConnection -= SendResourcePlaces;
        }
    }
}
