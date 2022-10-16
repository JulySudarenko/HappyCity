using System;
using UnityEngine;

namespace Code.Hit
{
    public class HitHandler : MonoBehaviour, IHit
    {
        public event Action<int, int> OnHitEnter;
        public event Action<int, int> OnHitExit;

        private void OnCollisionEnter(Collision other)
        {
            OnHitEnter?.Invoke(other.gameObject.GetInstanceID(), gameObject.GetInstanceID());
        }

        private void OnCollisionExit(Collision other)
        {
            OnHitExit?.Invoke(other.gameObject.GetInstanceID(), gameObject.GetInstanceID());
        }
    }
}
