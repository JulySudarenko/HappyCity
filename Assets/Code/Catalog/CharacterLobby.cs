using System;
using System.Collections.Generic;
using Code.Configs;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Code.Catalog
{
    public class CharacterLobby : IDisposable
    {
        private const string Gold = "GD";
        private readonly Dictionary<string, StoreItem> _characterCatalog = new Dictionary<string, StoreItem>();
        private readonly Dictionary<int, string> _charactersButtons = new Dictionary<int, string>();
        private readonly List<LineElementView> _elements = new List<LineElementView>();
        private readonly Dictionary<string, CatalogItem> _catalog;
        private readonly PlayerNamePanelView _enterNamePanel;
        private readonly Transform _charactersPanel;
        private readonly Transform _panel;
        private readonly LineElementView _lineElement;
        private readonly LineElementView _characterAllInfo;
        private readonly InventoryLobby _inventoryLobby;
        private StoreItem _storeItem;
        private string _characterName;
        private readonly Color _color = new Color(0.0f, 0.34f, 0.46f, 1.0f);
        private bool _isInfo;

        public CharacterLobby(PlayerNamePanelView enterNamePanel, Transform charactersPanel,
            LineElementView lineElement, Dictionary<string, CatalogItem> catalog, InventoryLobby inventoryLobby,
            Transform panel, LineElementView characterAllInfo)
        {
            _enterNamePanel = enterNamePanel;
            _charactersPanel = charactersPanel;
            _lineElement = lineElement;
            _panel = panel;
            _characterAllInfo = characterAllInfo;
            _catalog = catalog;
            _inventoryLobby = inventoryLobby;
            _enterNamePanel.NameInput.onEndEdit.AddListener(SetCharacterName);
            _enterNamePanel.AcceptNameButton.onClick.AddListener(CreateNewCharacter);
            _enterNamePanel.CancelButton.onClick.AddListener(() => _enterNamePanel.OpenClosePanel(false));
        }

        public void LoadCharacters()
        {
            ShowAllUserCharacters();
            CreateCharacterCatalog();
        }

        private void SetCharacterName(string newName)
        {
            _characterName = newName;
        }

        private void CreateCharacterCatalog()
        {
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
            {
                CatalogVersion = "0.1",
                StoreId = "ls2"
            }, result =>
            {
                foreach (var storeItem in result.Store)
                {
                    _characterCatalog.Add(storeItem.ItemId, storeItem);
                }

                CreateAddCharactersButtons();
            }, Error);
        }

        private void ShowAllUserCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    foreach (var character in result.Characters)
                    {
                        var characterLine = Object.Instantiate(_lineElement, _charactersPanel);
                        characterLine.gameObject.SetActive(true);
                        characterLine.TextUp.text = $"{character.CharacterName} {character.CharacterType}";
                        UpdateCharacterView(character.CharacterId, characterLine.TextDown);

                        _charactersButtons.Add(characterLine.GetInstanceID(), character.CharacterId);
                        _elements.Add(characterLine);
                        characterLine.Button.onClick.AddListener(() =>
                            OnCharacterButtonClick(characterLine.GetInstanceID()));

                        if (!_isInfo)
                        {
                            OnCharacterButtonClick(characterLine.GetInstanceID());
                            _isInfo = true;
                        }

                        if (!PlayerPrefs.HasKey(PreferenceKeys.AUTH_KEY_CHARACTER_ID))
                        {
                            PlayerPrefs.SetString(PreferenceKeys.AUTH_KEY_CHARACTER_ID, character.CharacterId);
                            PlayerPrefs.SetString(PreferenceKeys.AUTH_KEY_CHARACTER_TYPE, character.CharacterType);
                        }
                    }
                }, Error);
        }

        private void CreateAddCharactersButtons()
        {
            foreach (var storeItem in _characterCatalog)
            {
                var newAddButton = Object.Instantiate(_lineElement, _panel);
                newAddButton.gameObject.SetActive(true);
                newAddButton.Button.image.color = _color;
                newAddButton.TextUp.text = $"{_catalog[storeItem.Key].DisplayName} {_catalog[storeItem.Key].ItemId}";
                newAddButton.TextDown.text = $"{storeItem.Value.VirtualCurrencyPrices[Gold]} {Gold}";
                newAddButton.Button.onClick.AddListener(() => CreateCharacterPanel(storeItem.Value));
                _elements.Add(newAddButton);
            }
        }

        private void CreateCharacterPanel(StoreItem storeItem)
        {
            _storeItem = storeItem;
            _enterNamePanel.OpenClosePanel(true);
        }

        private void CreateNewCharacter()
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = _storeItem.ItemId,
                Price = (int) _storeItem.VirtualCurrencyPrices[Gold],
                VirtualCurrency = Gold,
            }, result => { CreateCharacterWithItemId(_storeItem.ItemId); }, Error);
        }

        private void CreateCharacterWithItemId(string itemId)
        {
            _enterNamePanel.AcceptNameButton.interactable = false;
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                CharacterName = _characterName,
                ItemId = itemId
            }, result =>
            {
                Debug.Log($"Get character type: {result.CharacterType}");
                _inventoryLobby.UpdateCurrency();
                UpdateCharacterStatistics(result.CharacterId);
            }, Error);
        }

        private void UpdateCharacterStatistics(string characterId)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>
                {
                    {"Level", 1},
                    {"Experience", 0}
                }
            }, result =>
            {
                _enterNamePanel.OpenClosePanel(false);
                _enterNamePanel.AcceptNameButton.interactable = true;
                UpdateCharacterSlots(characterId);
            }, Error);
        }

        private void UpdateCharacterSlots(string characterId)
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    foreach (var character in result.Characters)
                    {
                        if (character.CharacterId == characterId)
                        {
                            var characterLine = Object.Instantiate(_lineElement, _charactersPanel);
                            characterLine.gameObject.SetActive(true);
                            characterLine.TextUp.text = $"{character.CharacterName} {character.CharacterType}";

                            _charactersButtons.Add(characterLine.GetInstanceID(), character.CharacterId);
                            _elements.Add(characterLine);

                            characterLine.Button.onClick.AddListener(() =>
                                OnCharacterButtonClick(characterLine.GetInstanceID()));

                            OnCharacterButtonClick(characterLine.GetInstanceID());
                            UpdateCharacterView(character.CharacterId, characterLine.TextDown);
                        }
                    }
                }, Error);
        }

        private void UpdateCharacterView(string characterId, TMP_Text text)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = characterId
                },
                result => { text.text = $"{result.CharacterStatistics["Level"]} Level"; },
                Debug.LogError);
        }

        private void OnCharacterButtonClick(int buttonID)
        {
            foreach (var buttonLine in _charactersButtons)
            {
                if (buttonLine.Key == buttonID)
                {
                    UpdateCharacterFullInfoView(buttonLine.Value);
                }
            }
        }

        private void UpdateCharacterFullInfoView(string characterId)
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    foreach (var character in result.Characters)
                    {
                        if (character.CharacterId == characterId)
                        {
                            _characterAllInfo.TextUp.text = character.CharacterName;
                            _characterAllInfo.TextDown.text =
                                $"<b>ID</b> {character.CharacterId}\n<b>Type</b> {character.CharacterType}";

                            UpdateCharacterStatisticsInCharacterInfo(characterId);
                            PlayerPrefs.SetString(PreferenceKeys.AUTH_KEY_CHARACTER_ID, character.CharacterId);
                            PlayerPrefs.SetString(PreferenceKeys.AUTH_KEY_CHARACTER_TYPE, character.CharacterType);
                        }
                    }
                }, Error);
        }

        private void UpdateCharacterStatisticsInCharacterInfo(string characterId)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = characterId
                },
                result =>
                {
                    _characterAllInfo.TextDown.text +=
                        $"\n<b>Level</b> {result.CharacterStatistics["Level"]}\n<b>Experience</b> {result.CharacterStatistics["Experience"]}";
                },
                Debug.LogError);
        }

        private void Error(PlayFabError error)
        {
            _enterNamePanel.AcceptNameButton.interactable = true;
            Debug.LogError(error.GenerateErrorReport());
        }

        public void Dispose()
        {
            _enterNamePanel.NameInput.onEndEdit.RemoveListener(SetCharacterName);
            _enterNamePanel.AcceptNameButton.onClick.RemoveListener(CreateNewCharacter);
            _enterNamePanel.CancelButton.onClick.RemoveListener(() => _enterNamePanel.OpenClosePanel(false));
            foreach (var element in _elements)
            {
                element.Button.onClick.RemoveAllListeners();
            }
        }
    }
}
