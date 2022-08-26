using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Catalog
{
    public class CatalogManager
    {
        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
        private readonly CharacterLobby _characterLobby;

        public CatalogManager(Transform characterPanel, Transform otherPanel, PlayerNamePanelView enterNamePanel, TextElementView gold, TextElementView experience,
            LineElementView lineElement, LineElementView characterAllInfo)
        {
            var inventoryLobby = new InventoryLobby(gold, experience);
            _characterLobby = new CharacterLobby(enterNamePanel, characterPanel, lineElement, _catalog, inventoryLobby, otherPanel, characterAllInfo);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
        }
        
        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            HandleCatalog(result.Catalog);
            _characterLobby.LoadCharacters();
        }

        private void HandleCatalog(List<CatalogItem> catalog)
        {
            foreach (var item in catalog)
            {
                _catalog.Add(item.ItemId, item);
            }
        }
    }
}
