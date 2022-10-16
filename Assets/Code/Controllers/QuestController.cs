using System;
using Code.Configs;
using Code.Factory;
using Code.Interfaces;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;

namespace Code.Controllers
{
    internal class QuestController : IInitialization, ICleanup, IQuestState
    {
        public event Action<QuestState> OnStateChange;
        public Action<string, string, int> QuestStart;
        public Action<int> QuestDone;

        private readonly NpcSpawnHandler _npc;
        private readonly LineElementView _messagePanelView;
        private readonly BuildingConfig _buildingConfig;
        private MessagePanelViewHandler _messagePanelViewHandler;
        private readonly QuestResourcesChecker _resourcesChecker;
        private readonly string _startMessage = "There is no place to live. Can you help to build a house for me?";

        private readonly string _waitingMessage =
            "That is not enough. When you get wood, stone and food? You can get it in the forest not far from here. I'm waiting...";

        private readonly string _getResMessage = "Now you can build a house!";
        private readonly string _doneMessage = "I'm so happy! Thank you!";
        private readonly string _questName = "House";
        private readonly string _questInfo = "House for homeless man";
        private readonly string _acceptTaskButtonText = "Accept task";
        private readonly string _buildButtonText = "Build";

        private QuestState _state;
        private readonly int _characterID;
        private readonly int _npcID;

        private bool _isActivate;

        public QuestController(NpcSpawnHandler npcController, int characterID, LineElementView messagePanelView,
            BuildingConfig buildingConfig, QuestResourcesChecker resourcesChecker)
        {
            _npc = npcController;
            _npcID = _npc.NPCID;
            _characterID = characterID;
            _messagePanelView = messagePanelView;
            _buildingConfig = buildingConfig;
            _resourcesChecker = resourcesChecker;
            _state = QuestState.None;
            _state = QuestState.Start;
        }

        public void Initialize()
        {
            _npc.HitHandler.OnHitEnter += StartDialog;
            _npc.HitHandler.OnHitExit += EndDialog;
        }

        private void EndDialog(int ID, int selfID)
        {
            if (ID == _characterID)
            {
                _messagePanelViewHandler.ActivationPanel(false);
            }
        }

        private void StartDialog(int ID, int selfID)
        {
            if (ID == _characterID)
            {
                switch (_state)
                {
                    case QuestState.None:

                        break;
                    case QuestState.Start:
                        _messagePanelViewHandler = new MessagePanelViewHandler(_messagePanelView);
                        _messagePanelViewHandler.ActivationPanel(true);
                        _messagePanelViewHandler.ChangeTextOnButton(_acceptTaskButtonText);
                        _messagePanelViewHandler.AcceptButton.gameObject.SetActive(true);
                        var message =
                            $"{_startMessage} wood {_buildingConfig.WoodCost}, stone {_buildingConfig.FoodCost}, food {_buildingConfig.FoodCost}";
                        _messagePanelViewHandler.ShowMessage(message);
                        if (!_isActivate)
                        {
                            _messagePanelViewHandler.AcceptButton.onClick.AddListener(AcceptTask);
                            _isActivate = true;
                        }

                        break;
                    case QuestState.Waiting:
                        _messagePanelViewHandler.ActivationPanel(true);
                        _messagePanelViewHandler.ChangeTextOnButton(_buildButtonText);
                        _messagePanelViewHandler.AcceptButton.gameObject.SetActive(false);
                        if (!_isActivate)
                        {
                            _messagePanelViewHandler.AcceptButton.onClick.AddListener(Build);
                            _isActivate = true;
                        }

                        if (Check())
                        {
                            _messagePanelViewHandler.ShowMessage(_getResMessage);
                            _messagePanelViewHandler.AcceptButton.interactable = true;
                            _messagePanelViewHandler.AcceptButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            _messagePanelViewHandler.AcceptButton.gameObject.SetActive(false);
                            _messagePanelViewHandler.ShowMessage(_waitingMessage);
                        }
                        break;
                    case QuestState.Done:
                        _messagePanelViewHandler.ShowMessage(_doneMessage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool Check()
        {
            if (_resourcesChecker.CheckResources(ResourcesType.Wood, _buildingConfig.WoodCost) &&
                _resourcesChecker.CheckResources(ResourcesType.Food, _buildingConfig.FoodCost) &&
                _resourcesChecker.CheckResources(ResourcesType.Stone, _buildingConfig.StoneCost) &&
                _resourcesChecker.CheckResources(ResourcesType.Gold, _buildingConfig.GoldCost))
            {
                return true;
            }

            return false;
        }

        private void Build()
        {
            _resourcesChecker.SpendResources(ResourcesType.Wood, _buildingConfig.WoodCost);
            _resourcesChecker.SpendResources(ResourcesType.Food, _buildingConfig.FoodCost);
            _resourcesChecker.SpendResources(ResourcesType.Stone, _buildingConfig.StoneCost);
            _resourcesChecker.GrandResources(ResourcesType.Gold, _buildingConfig.GoldReward);
            _messagePanelViewHandler.AcceptButton.interactable = false;
            _messagePanelViewHandler.AcceptButton.gameObject.SetActive(false);
            _state = QuestState.Done;
            _isActivate = false;
            OnStateChange?.Invoke(_state);
                QuestDone.Invoke(_npcID);
        }

        private void AcceptTask()
        {
            _messagePanelViewHandler.AcceptButton.interactable = false;
            _messagePanelViewHandler.AcceptButton.onClick.RemoveListener(AcceptTask);
            _messagePanelViewHandler.AcceptButton.gameObject.SetActive(false);
            _messagePanelViewHandler.ActivationPanel(false);
            var questMessage =
                $"{_questInfo}\nwood {_buildingConfig.WoodCost}\nstone {_buildingConfig.FoodCost}\nfood {_buildingConfig.FoodCost}";
            QuestStart?.Invoke(_questName, questMessage, _npcID);
            _isActivate = false;
            _state = QuestState.Waiting;

        }

        public void Cleanup()
        {
            _npc.HitHandler.OnHitEnter -= StartDialog;
            _npc.HitHandler.OnHitExit -= EndDialog;
        }
    }
}
