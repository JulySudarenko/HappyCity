using System;
using UnityEngine;

namespace Code.Factory
{
    internal class HitHandler : MonoBehaviour
    {
        public Action<int, int> IsHit;

        private void OnCollisionEnter(Collision other)
        {
            IsHit?.Invoke(other.gameObject.GetInstanceID(), gameObject.GetInstanceID());
            Debug.Log($"Hit {other.gameObject.GetInstanceID()}");
        }
    }
}
