﻿using Code.Configs;
using Code.Controllers;
using PlayFab;
using PlayFab.ClientModels;
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
            //GetCurrentCharacter();
        }

        private void GetCurrentCharacter()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    foreach (var character in result.Characters)
                    {
                        if (character.CharacterId == _characterPlayFabID)
                        {
                            //OnSelectedCharacter(character.CharacterType);
                        }

                        // var characterLine = Object.Instantiate(_lineElement, _characterSelectedPanel);
                        // characterLine.gameObject.SetActive(true);
                        // characterLine.TextUp.text = $"{character.CharacterName}";
                        // characterLine.Button.onClick.AddListener(() => SelectCharacter(character.CharacterType));
                        // UpdateCharacterView(character.CharacterId, characterLine.TextDown);
                        // _lineElements.Add(characterLine);
                    }
                }, Debug.LogError);
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
            Character = new CharacterModel(_factory, networkSynchronizationController);
        }
    }
}
