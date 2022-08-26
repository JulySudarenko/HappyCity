using System;
using UnityEngine;

namespace Code.Factory
{
    internal class HitHandler : MonoBehaviour
    {
        public Action<int, int> _IsHit;

        private void OnCollisionEnter(Collision other)
        {
            _IsHit?.Invoke(other.gameObject.GetInstanceID(), gameObject.GetInstanceID());
        }
    }
}
