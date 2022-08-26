using System.Collections.Generic;
using Code.Controllers;
using Code.Factory;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Code.ViewHandlers
{
    internal class CharacterSelectedPanelViewHandler
    {
        private readonly Transform _characterSelectedPanel;
        private readonly LineElementView _lineElement;
        private readonly CharacterSpawnHandler _spawnHandler;

        private readonly List<LineElementView> _lineElements = new List<LineElementView>();
        
        public CharacterSelectedPanelViewHandler(Transform characterSelectedPanel, LineElementView lineElement, CharacterSpawnHandler spawnHandler)
        {
            _characterSelectedPanel = characterSelectedPanel;
            _lineElement = lineElement;
            _spawnHandler = spawnHandler;
        }

        public void Init()
        {
            ShowAllUserCharacters();
            
        }

        private void ShowAllUserCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    foreach (var character in result.Characters)
                    {
                        var characterLine = Object.Instantiate(_lineElement, _characterSelectedPanel);
                        characterLine.gameObject.SetActive(true);
                        characterLine.TextUp.text = $"{character.CharacterName}";
                        characterLine.Button.onClick.AddListener(() => SelectCharacter(character.CharacterType));
                        UpdateCharacterView(character.CharacterId, characterLine.TextDown);
                        _lineElements.Add(characterLine);
                    }
                }, Debug.LogError);
        }

        private void UpdateCharacterView(string characterId, TMP_Text text)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = characterId
                },
                result =>
                {
                    text.text = $"<b>Level</b>\n{result.CharacterStatistics["Level"]}" +
                                $"\n<b>Experience</b>\n{result.CharacterStatistics["Experience"]}";
                },
                Debug.LogError);
        }

        private void SelectCharacter(string type)
        {
            _spawnHandler.IsCharacterSelected?.Invoke(type);
            _characterSelectedPanel.gameObject.SetActive(false);
        }


        public void Cleanup()
        {
            for (int i = 0; i < _lineElements.Count; i++)
            {
                _lineElements[i].Button.onClick.RemoveAllListeners();
            }
        }
    }
}
