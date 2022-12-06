using Photon.Pun;
using UnityEngine;


namespace Code.Factory
{
    internal sealed class CharacterFactory : ICharacterFactory
    {
        private readonly Transform _character;
        private readonly Transform _spawnPoint;

        public CharacterFactory(Transform character, Transform spawnPoint)
        {
            _character = character;
            _spawnPoint = spawnPoint;
        }

        public GameObject SpawnCharacter()
        {
            var number = PhotonNetwork.LocalPlayer.ActorNumber;
            var player = PhotonNetwork.Instantiate(_character.gameObject.name,
                _spawnPoint.GetChild(Mathf.Abs(number)).position, Quaternion.identity);
            return player;
        }

    }
}
