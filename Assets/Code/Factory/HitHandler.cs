using System;
using UnityEngine;

namespace Code.Factory
{
    public class HitHandler : MonoBehaviour
    {
        public Action<int, string> IsHit;

        private void OnCollisionEnter(Collision other)
        {
                IsHit?.Invoke(other.gameObject.GetInstanceID(), other.gameObject.tag);
        }
    }
}
