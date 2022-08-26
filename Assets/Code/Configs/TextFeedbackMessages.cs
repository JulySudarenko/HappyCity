using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "TextFeedbackMessages", menuName = "Texts/TextFeedbackMessages", order = 0)]
    public sealed class TextFeedbackMessages : ScriptableObject
    {
        public string SingUpMessage = "Please, sing up";
        public string PlayerNameNotEnter = "Player name not entered";
        public string WaitingMessage = "Waiting for connection...";
        public string SuccessMessage = "Success: ";
        public string FailMessage = "Fail: ";
        public string ErrorMessage = "Something went wrong: ";
    }
}
