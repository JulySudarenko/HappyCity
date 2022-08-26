using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


namespace Code.Catalog
{
    public class InventoryLobby
    {
        private readonly TextElementView _gold;
        private readonly TextElementView _experience;

        public InventoryLobby(TextElementView gold, TextElementView experience)
        {
            _gold = gold;
            _experience = experience;
            UpdateCurrency();
        }

        public void UpdateCurrency()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                success =>
                {
                    foreach (KeyValuePair<string, int> currency in success.VirtualCurrency)
                    {
                        if (currency.Key == "GD")
                            _gold.ShowCurrency(currency.Key, currency.Value.ToString());
                        else if (currency.Key == "EX")
                            _experience.ShowCurrency(currency.Key, currency.Value.ToString());
                        else
                        {
                            Debug.Log(currency);
                        }
                    }
                },
                error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
        }
    }
}
