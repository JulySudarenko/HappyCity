using UnityEngine;

namespace Code.Configs
{
    public class QuestConfig : ScriptableObject
    {
        [SerializeField] private string _questName = "House";
        [SerializeField] private string _questInfo = "House for homeless man";
        [SerializeField]
        private string _startMessage = "There is no place to live. Can you help to build a house for me?";

        [SerializeField] private string _waitingMessage =
            "That is not enough. When you get wood, stone and food? You can get it in the forest not far from here. I'm waiting...";

        [SerializeField] private string _getResMessage = "Now you can build a house!";
        [SerializeField] private string _doneMessage = "I'm so happy! Thank you!";

        [SerializeField] private string _acceptTaskButtonText = "Accept task";
        [SerializeField] private string _buildButtonText = "Build";

        public string StartMessage => _startMessage;

        public string WaitingMessage => _waitingMessage;

        public string GETResMessage => _getResMessage;

        public string DoneMessage => _doneMessage;

        public string QuestInfo => _questInfo;

        public string QuestName => _questName;

        public string AcceptTaskButtonText => _acceptTaskButtonText;

        public string BuildButtonText => _buildButtonText;
    }
}
