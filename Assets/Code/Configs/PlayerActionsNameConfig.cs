using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "PlayerActionsNames", menuName = "Texts/PlayerActionsNames", order = 0)]
    public sealed class PlayerActionsNameConfig : ScriptableObject
    {
        public string Speed = "Speed";
        public string Jump = "Jump";
        public string Work = "Work";
        public string Horizontal = "Horizontal";
        public string Vertical = "Vertical";
    }
}
