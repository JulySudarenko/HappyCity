using System;
using System.Collections.Generic;
using Code.Configs;
using Code.Interfaces;
using Code.Network;
using Code.NPC;
using Code.ResourcesC;
using Code.ResourcesSpawn;
using Code.View;
using Code.ViewHandlers;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


namespace Code.Quest
{
    internal class QuestController : IQuestState, ICleanup
    {
        public event Action<QuestState> OnStateChange;
        public event Action<string, string, int> OnQuestStart;
        public event Action<int> OnQuestDone;
        public event Action<bool> OnDialog;
        private readonly List<Vector3> _questsInWork;
        private readonly NetworkSynchronizationController _networkSynchronization;
        private readonly IKeeper _npcHappiness;
        private readonly NpcSpawnHandler _npc;
        private readonly QuestNpcConfig _questConfig;
        private readonly BuildingConfig _buildingConfig;
        private readonly DialogPanelViewHandler _dialogPanelViewHandler;
        private readonly ResourcesCheckUnionController _resourcesCheckUnionController;
        private readonly HappyLineController _happyLineController;
        private readonly string _nickName;
        private readonly int _characterID;
        private readonly int _npcID;
        public Vector3 QuestNumber { get; }
        private QuestState _state;
        private string _message;
        private bool _isAccepted;

        public QuestController(NpcSpawnHandler npcController, int characterID, string nickName,
            ResourcesCheckUnionController resourcesCheckUnionController, QuestNpcConfig questConfig,
            Canvas canvas, IKeeper npcHappiness, HappyLineController happyLineController, Vector3 questNumber,
            DialogPanelView dialogPanelView, List<Vector3> questsInWork,
            NetworkSynchronizationController networkSynchronization)
        {
            _nickName = nickName;
            _npc = npcController;
            _npcID = _npc.NpcId;
            _characterID = characterID;
            _resourcesCheckUnionController = resourcesCheckUnionController;
            _questConfig = questConfig;
            _npcHappiness = npcHappiness;
            _happyLineController = happyLineController;
            QuestNumber = questNumber;
            _questsInWork = questsInWork;
            _networkSynchronization = networkSynchronization;
            _dialogPanelViewHandler = new DialogPanelViewHandler(dialogPanelView, canvas);
            _buildingConfig = _questConfig.BuildingConfig;

            _state = QuestState.None;
            _state = QuestState.Start;
            _npc.HitHandler.OnHitEnter += StartDialog;
            _npc.HitHandler.OnHitExit += EndDialog;
        }

        private void EndDialog(int ID, int selfID)
        {
            if (ID == _characterID && _npc.IsTalking)
            {
                _dialogPanelViewHandler.ActivationPanel(false);
                _npc.OnDialog(false);
                OnDialog?.Invoke(false);
            }
        }

        private void StartDialog(int ID, int selfID)
        {
            if (ID == _characterID && !_npc.IsTalking)
            {
                _dialogPanelViewHandler.ActivationPanel(true);
                OnDialog?.Invoke(true);
                _npc.OnDialog(true);

                switch (_state)
                {
                    case QuestState.None:
                        _dialogPanelViewHandler.ActivationPanel(false);
                        break;
                    case QuestState.Start:
                        _dialogPanelViewHandler.AcceptButton.onClick.AddListener(() => AcceptTask(ID, selfID));
                        _message =
                            $"{_questConfig.StartMessage} wood {_buildingConfig.WoodCost}, stone {_buildingConfig.StoneCost}, food {_buildingConfig.FoodCost}";
                        _dialogPanelViewHandler.ShowMessage(_message);
                        _dialogPanelViewHandler.AcceptButton.gameObject.SetActive(true);
                        _state = QuestState.Wait;
                        break;
                    case QuestState.Wait:

                        break;
                    case QuestState.Check:
                        if (Check())
                        {
                            _dialogPanelViewHandler.ShowMessage(_questConfig.GETResMessage);
                            _dialogPanelViewHandler.BuildButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            _dialogPanelViewHandler.ShowMessage(_message);
                            _dialogPanelViewHandler.BuildButton.gameObject.SetActive(false);
                        }

                        break;
                    case QuestState.Done:
                        _dialogPanelViewHandler.ShowMessage(_questConfig.DoneMessage);
                        break;
                    case QuestState.Busy:
                        _dialogPanelViewHandler.ShowMessage("This quest is already taken");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool Check()
        {
            return _resourcesCheckUnionController.CheckResources(ResourcesType.Wood, _buildingConfig.WoodCost) &&
                   _resourcesCheckUnionController.CheckResources(ResourcesType.Food, _buildingConfig.FoodCost) &&
                   _resourcesCheckUnionController.CheckResources(ResourcesType.Stone, _buildingConfig.StoneCost) &&
                   _resourcesCheckUnionController.CheckResources(ResourcesType.Gold, _buildingConfig.GoldCost);
        }

        private void Build(int ID, int selfID)
        {
            _resourcesCheckUnionController.SpendResources(ResourcesType.Wood, _buildingConfig.WoodCost);
            _resourcesCheckUnionController.SpendResources(ResourcesType.Food, _buildingConfig.FoodCost);
            _resourcesCheckUnionController.SpendResources(ResourcesType.Stone, _buildingConfig.StoneCost);
            _resourcesCheckUnionController.GrandResources(ResourcesType.Gold, _buildingConfig.GoldReward);
            _resourcesCheckUnionController.GrandResources(ResourcesType.Happiness, _questConfig.BonusHappiness);

            _dialogPanelViewHandler.BuildButton.gameObject.SetActive(false);
            _dialogPanelViewHandler.BuildButton.onClick.RemoveListener(() => Build(ID, selfID));

            _npcHappiness.Add(_questConfig.BonusHappiness);
            _happyLineController.ChangeHappiness(_questConfig.StartHappiness, _questConfig.BonusHappiness);
            _state = QuestState.Done;

            PhotonNetwork.RaiseEvent(123, QuestNumber,
                new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                new SendOptions() {Reliability = true});
            _networkSynchronization.ChangeQuestState(QuestNumber, QuestState.Done);

            string message = JsonUtility.ToJson(new NetScoreTable()
                {Name = _nickName, Score = _questConfig.BonusHappiness});

            PhotonNetwork.RaiseEvent(131, message,
                new RaiseEventOptions() {Receivers = ReceiverGroup.All},
                new SendOptions() {Reliability = true});

            for (int i = 0; i < _questsInWork.Count; i++)
            {
                if (_questsInWork[i] == QuestNumber)
                {
                    _questsInWork.Remove(_questsInWork[i]);
                }
            }
            
            OnStateChange?.Invoke(_state);
            OnQuestDone?.Invoke(_npcID);
            _npc.OnDialog(false);

            StartDialog(ID, selfID);
        }

        private void AcceptTask(int ID, int selfID)
        {
            if (!_isAccepted)
            {
                _dialogPanelViewHandler.AcceptButton.gameObject.SetActive(false);
                _dialogPanelViewHandler.AcceptButton.onClick.RemoveListener(() => AcceptTask(ID, selfID));

                var questMessage =
                    $"{_questConfig.QuestInfo}\nwood {_buildingConfig.WoodCost}\nstone {_buildingConfig.StoneCost}\nfood {_buildingConfig.FoodCost}";
                OnQuestStart?.Invoke(_questConfig.QuestName, questMessage, _npcID);

                _state = QuestState.Check;
                _isAccepted = true;

                PhotonNetwork.RaiseEvent(122, QuestNumber,
                    new RaiseEventOptions() {Receivers = ReceiverGroup.Others},
                    new SendOptions() {Reliability = true});
                _networkSynchronization.ChangeQuestState(QuestNumber, QuestState.Busy);

                _message =
                    $"{_questConfig.WaitingMessage}, wood {_buildingConfig.WoodCost}, stone {_buildingConfig.StoneCost}, food {_buildingConfig.FoodCost}";

                _dialogPanelViewHandler.BuildButton.gameObject.SetActive(false);
                _dialogPanelViewHandler.BuildButton.onClick.AddListener(() => Build(ID, selfID));
                _questsInWork.Add(QuestNumber);

                OnStateChange?.Invoke(_state);
                _npc.OnDialog(false);
                StartDialog(ID, selfID);
            }
        }

        public void OnSomebodyChangeQuest(QuestState state)
        {
            if (state == QuestState.Start)
            {
                _state = QuestState.Start;
                OnStateChange?.Invoke(_state);
            }

            if (state == QuestState.Busy)
            {
                var message = "My task is already taken";
                _dialogPanelViewHandler.ShowMessage(_message);
                _dialogPanelViewHandler.AcceptButton.gameObject.SetActive(false);
                _dialogPanelViewHandler.AcceptButton.onClick.RemoveAllListeners();
                _state = QuestState.Busy;
                OnStateChange?.Invoke(_state);
            }

            if (state == QuestState.Done)
            {
                _npcHappiness.Add(_questConfig.BonusHappiness);
                _happyLineController.ChangeHappiness(_questConfig.StartHappiness, _questConfig.BonusHappiness);
                _state = QuestState.Done;

                for (int i = 0; i < _questsInWork.Count; i++)
                {
                    if (_questsInWork[i] == QuestNumber)
                    {
                        _questsInWork.Remove(_questsInWork[i]);
                    }
                }

                OnStateChange?.Invoke(QuestState.Done);
            }
        }

        public void Cleanup()
        {
            _npc.HitHandler.OnHitEnter -= StartDialog;
            _npc.HitHandler.OnHitExit -= EndDialog;
        }
    }
}
