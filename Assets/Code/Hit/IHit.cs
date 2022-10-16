using System;

namespace Code.Hit
{
    internal interface IHit
    {
        event Action<int, int> OnHitEnter;
        event Action<int, int> OnHitExit;
    }
}
