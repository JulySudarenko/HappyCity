using System.Collections.Generic;
using Code.Buildings;
using Code.Configs;
using Code.Hit;
using Code.Interfaces;
using Code.Quest;
using Code.Timer;
using UnityEngine;


namespace Code.NPC
{
    internal class NpcController : IExecute, IFixedExecute, ICleanup
    {
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Stay = Animator.StringToHash("Stay");
        private readonly List<Vector3> _startPointsList;
        private readonly NpcSpawnHandler _npc;
        private readonly NonPlayerCharacterConfig _config;
        private readonly Transform _player;
        private readonly IQuestState _state;
        private readonly BuildingSpawnHandler _buildingSpawnHandler;
        private readonly Vector3 _startPoint;
        private readonly int _playerID;
        private ITimeRemaining _timeRemaining;
        private Vector3 _target;
        private Vector3 _newVelocity;
        private float _horizontal;
        private float _vertical;
        private bool _hasTarget;

        public NpcController(NonPlayerCharacterConfig npcConfig, NpcSpawnHandler npc, Transform player, int playerID)
        {
            _config = npcConfig;
            _npc = npc;
            _player = player;
            _playerID = playerID;

            _npc.HitHandler.OnHitEnter += LookAtPlayer;
        }

        public NpcController(NonPlayerCharacterConfig npcConfig, NpcSpawnHandler npc, Transform player, int playerID,
            IQuestState state, BuildingSpawnHandler buildingSpawnHandler, Vector3 startPoint,
            List<Vector3> startPointsList)
        {
            _config = npcConfig;
            _npc = npc;
            _player = player;
            _playerID = playerID;
            _state = state;
            _buildingSpawnHandler = buildingSpawnHandler;
            _startPointsList = startPointsList;
            _startPoint = startPoint;
            RemoveStartPoint(startPoint);
            //_buildingSpawnHandler.BuildingEnter.OnHitEnter += EnterInBuilding;
            _npc.HitHandler.OnHitEnter += LookAtPlayer;
            //_state.OnStateChange += OnQuestDone;
            _buildingSpawnHandler.BuildingIsDone += OnGetTarget;
        }

        private void OnQuestDone(QuestState state)
        {
            if (state == QuestState.Done)
            {
                var target = new Vector3(-30.0f + Random.Range(1, 3), 0.0f, -30.0f);
                //var target = _buildingSpawnHandler.BuildingEnterPosition;
                //OnGetTarget(_npc.NpcTransform.position, target);
            }
        }

        private void RemoveStartPoint(Vector3 point)
        {
            for (int i = 0; i < _startPointsList.Count; i++)
            {
                if (point == _startPointsList[i])
                {
                    _startPointsList.Remove(_startPointsList[i]);

                }
            }
        }

        //private void OnGetTarget(Vector3 start, Vector3 target)
        private void OnGetTarget(IHit hit, Vector3 target)
        {
            _target = target;
            _npc.NpcTransform.position = _startPoint;
            _npc.NpcTransform.gameObject.SetActive(true);
            _timeRemaining = new TimeRemaining(WaitAfterActivation, 5.0f);
            _timeRemaining.AddTimeRemaining();
            //_hitBuilding.OnHitEnter += EnterInBuilding;
            _buildingSpawnHandler.BuildingIsDone -= OnGetTarget;
        }

        private void WaitAfterActivation()
        {
            _hasTarget = true;
            _startPointsList.Add(_startPoint);
            _timeRemaining.RemoveTimeRemaining();
        }

        // private void EnterInBuilding(int characterID, int otherID)
        // {
        //     if (characterID == _npc.NpcColliderID)
        //     {
        //         Debug.Log($"HIT {_target}");
        //         _timeRemaining = new TimeRemaining(Deactivate, 5.0f);
        //         _timeRemaining.AddTimeRemaining();
        //         _hasTarget = false;
        //         _hitBuilding.OnHitEnter -= EnterInBuilding;
        //     }
        // }

        public void Execute(float deltaTime)
        {
            if (_hasTarget)
            {
                _npc.NpcAnimator.SetTrigger(Walk);
            }
            else
            {
                _npc.NpcAnimator.SetTrigger(Stay);
            }
            
            if (_hasTarget)
            {
                GoToTarget();
            }
        }

        public void FixedExecute(float deltaTime)
        {
            // if (_hasTarget)
            // {
            //     GoToTarget();
            // }
        }

        private void GoToTarget()
        {
            _npc.NavMeshAgent.SetDestination(_target);
            
            // var direction = (_target - _npc.NpcTransform.position).normalized;
            //
            // _horizontal = direction.x;
            // _vertical = direction.z;
            //
            // _npc.NpcTransform.LookAt(_target);
            //
            // Vector3 relativePos = _newVelocity;
            // var angle = Vector3.Angle(Vector3.forward, relativePos);
            // var axis = Vector3.Cross(Vector3.forward, relativePos);
            // _npc.NpcTransform.rotation = Quaternion.AngleAxis(angle, axis);
            //
            // _newVelocity.Set(_horizontal, 0.0f, _vertical);
            // _npc.NpcRigidbody.AddForce(_newVelocity * _config.Speed);

            if (Mathf.Abs(_npc.NpcTransform.position.x - _target.x) < 3.0f &&
                Mathf.Abs(_npc.NpcTransform.position.z - _target.z) < 3.0f)
            {
                _timeRemaining = new TimeRemaining(Deactivate, 5.0f);
                _timeRemaining.AddTimeRemaining();
                _hasTarget = false;
            }
            
            _npc.HitHandler.OnHitEnter -= LookAtPlayer;
        }

        private void LookAtPlayer(int ID, int selfID)
        {
            if (ID == _playerID)
            {
                _npc.NpcTransform.LookAt(_player);
            }
        }

        private void Deactivate()
        {
            _npc.NpcTransform.gameObject.SetActive(false);
            _timeRemaining.RemoveTimeRemaining();
        }

        public void Cleanup()
        {
            _npc.HitHandler.OnHitEnter -= LookAtPlayer;
        }
    }
}
