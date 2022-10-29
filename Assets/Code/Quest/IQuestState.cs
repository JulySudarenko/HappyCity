using System;

namespace Code.Quest
{
    internal interface IQuestState
    {
        event Action<QuestState> OnStateChange;
        event Action<string, string, int> OnQuestStart;
        event Action<int> OnQuestDone;
        event Action<bool> OnDialog;
    }
}
