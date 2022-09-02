using System;
using Code.Configs;
using Code.Interfaces;
using Code.UserInput;
using UnityEngine;

namespace Code.Factory
{
    internal class CharacterSpawnHandler : ICleanup
    {
        public Action<string> IsCharacterSelected;
        public Action IsCharacterCreated;
        private readonly PlayerConfig _playerConfig;
        private readonly InputInitialization _input;
        private ICharacterFactory _factory;
        private Transform _selectedCharacter;
        public CharacterInitialization Character { get; private set; }

        public CharacterSpawnHandler(PlayerConfig playerConfig, InputInitialization input)
        {
            _playerConfig = playerConfig;
            _input = input;
            IsCharacterSelected += OnSelectedCharacter;
        }

        private void OnSelectedCharacter(string type)
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
            Character = new CharacterInitialization(_factory, _input, _playerConfig);
            IsCharacterCreated?.Invoke();
        }

        public void Cleanup()
        {
            IsCharacterSelected -= OnSelectedCharacter;
        }
    }
}
