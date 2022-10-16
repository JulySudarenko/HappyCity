using System;

namespace Code.Controllers
{
    internal interface IQuestState
    {
        event Action<QuestState> OnStateChange;
    }
}
