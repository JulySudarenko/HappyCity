using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "PreferenceKeys", menuName = "Texts/PreferenceKeys", order = 0)]
    public sealed class PreferenceKeysConfig : ScriptableObject
    {
        private const string AuthKey = "IdKey";
        private const string PlayerNamePrefKey = "PlayerName";
        private const string AuthKeyName = "IdName";
        private const string AuthKeyPassword = "IdPassword";
    }
}
