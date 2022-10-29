using Code.Assistance;
using Code.Configs;
using Code.Hit;
using UnityEngine;

namespace Code.NPC
{
    internal class NpcSpawnHandler
    {
        public Transform NpcTransform { get; }
        public IHit HitHandler { get; }
        public Rigidbody NpcRigidbody { get; }
        public Animator NpcAnimator { get; }
        public Renderer Renderer { get; }
        public int NpcId { get; }

        public NpcSpawnHandler(NonPlayerCharacterConfig config)
        {
            var startPosition = config.SpawnPoints.GetChild(Random.Range(0, config.SpawnPoints.childCount))
                .position;
            NpcTransform = Object.Instantiate(config.Prefab, startPosition, Quaternion.identity);
            NpcRigidbody = NpcTransform.gameObject.GetOrAddComponent<Rigidbody>();
            NpcAnimator = NpcTransform.gameObject.GetOrAddComponent<Animator>();
            HitHandler = NpcTransform.gameObject.GetOrAddComponent<TriggerHandler>();
            Renderer = NpcTransform.GetComponent<Renderer>();
            NpcId = NpcTransform.gameObject.GetInstanceID();
        }
    }
}
