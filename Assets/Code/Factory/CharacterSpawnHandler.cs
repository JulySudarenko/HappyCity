using Code.Configs;
using Code.Network;
using UnityEngine;

namespace Code.Factory
{
    internal class CharacterSpawnHandler
    {
        private readonly string _characterPlayFabID;
        private readonly PlayerConfig _playerConfig;
        private ICharacterFactory _factory;
        private Transform _selectedCharacter;
        public CharacterModel Character { get; private set; }

        public CharacterSpawnHandler(PlayerConfig playerConfig,
            NetworkSynchronizationController networkSynchronizationController)
        {
            _playerConfig = playerConfig;
            _characterPlayFabID = PlayerPrefs.GetString(PreferenceKeys.AUTH_KEY_CHARACTER_ID);
            OnSelectedCharacter(PlayerPrefs.GetString(PreferenceKeys.AUTH_KEY_CHARACTER_TYPE),
                networkSynchronizationController);
        }

        private void OnSelectedCharacter(string type, NetworkSynchronizationController networkSynchronizationController)
        {
            switch (type)
            {
                case "ch01":
                    _selectedCharacter = _playerConfig.FemalePrefab;
                    break;
                case "ch02":
                    _selectedCharacter = _playerConfig.MalePrefab;
                    break;
                default:
                    Debug.Log($"No character with type {type}");
                    break;
            }

            _factory = new CharacterFactory(_selectedCharacter, _playerConfig.SpawnPoints);
            Character = new CharacterModel(_factory, networkSynchronizationController, _playerConfig.FeetCollider);
        }
    }
}
