using System.Collections.Generic;
using Code.Interfaces;
using Code.ResourcesC;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Factory
{
    internal class CharacterGrandGoldController : ICleanup
    {
        private readonly ResourceCounterController _goldCounterController;

        public CharacterGrandGoldController(ResourceCounterController goldCounterController)
        {
            _goldCounterController = goldCounterController;
            ShowFromLobby();
        }

        private void GrandResources(int value)
        {
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
                {
                    VirtualCurrency = "GD", 
                    Amount = value
                },  
                success =>
                {
                    new ModifyUserVirtualCurrencyResult();
                    PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                        success =>
                        {
                        },
                        error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
                }, 
                error => { Debug.LogError($"Add gold Failed: {error}"); });
        }


        private void ShowFromLobby()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                success =>
                {
                    foreach (KeyValuePair<string, int> currency in success.VirtualCurrency)
                    {
                        if (currency.Key == "GD")
                        {
                            _goldCounterController.OnGrandResource(currency.Value);
                        }
                    }
                    
                    _goldCounterController.Grand += GrandResources;
                },
                error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
        }

        public void GetWinReward(int value)
        {
            GrandResources(value);
        }
        
        public void Cleanup()
        {
            _goldCounterController.Grand -= GrandResources;
        }
    }
}
