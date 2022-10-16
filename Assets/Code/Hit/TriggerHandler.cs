using System;
using UnityEngine;

namespace Code.Hit
{
    public class TriggerHandler : MonoBehaviour, IHit
    {
        public event Action<int, int> OnHitEnter;
        public event Action<int, int> OnHitExit;

        private void OnTriggerEnter(Collider other)
        {
            OnHitEnter?.Invoke(other.GetInstanceID(), gameObject.GetInstanceID());
        }

        private void OnTriggerExit(Collider other)
        {
            OnHitExit?.Invoke(other.GetInstanceID(), gameObject.GetInstanceID());
        }
    }
}
