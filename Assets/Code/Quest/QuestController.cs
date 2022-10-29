using System;
using Code.Configs;
using Code.Interfaces;
using Code.NPC;
using Code.ResourcesC;
using Code.ResourcesSpawn;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;


namespace Code.Quest
{
    internal class QuestController : IInitialization, IQuestState, ICleanup
    {
        public event Action<QuestState> OnStateChange;
        public event Action<string, string, int> OnQuestStart;
        public event Action<int> OnQuestDone;
        public event Action<bool> OnDialog;

        private readonly NpcSpawnHandler _npc;
        private readonly QuestNpcConfig _questConfig;
        private readonly BuildingConfig _buildingConfig;
        private readonly MessagePanelViewHandler _messagePanelViewHandler;
        private readonly ResourcesCheckUnionController _resourcesCheckUnionController;
        private readonly int _characterID;
        private readonly int _npcID;
        
        private QuestState _state;
        private string _message;
        private bool _isAccepted;

        public QuestController(NpcSpawnHandler npcController, int characterID,
            ResourcesCheckUnionController resourcesCheckUnionController, QuestNpcConfig questConfig,
            LineElementView messagePanelView, Canvas canvas)
        {
            _npc = npcController;
            _npcID = _npc.NpcId;
            _characterID = characterID;
            _resourcesCheckUnionController = resourcesCheckUnionController;
            _questConfig = questConfig;
            _messagePanelViewHandler = new MessagePanelViewHandler(messagePanelView, canvas);
            _buildingConfig = _questConfig.BuildingConfig;

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
                OnDialog?.Invoke(false);
            }
        }

        private void StartDialog(int ID, int selfID)
        {
            if (ID == _characterID)
            {
                _messagePanelViewHandler.ActivationPanel(true);
                OnDialog?.Invoke(true);
                
                switch (_state)
                {
                    case QuestState.None:
                        _messagePanelViewHandler.ActivationPanel(false);
                        //тут можно показывать сообщение, что кто-то другой уже работает над задачей.
                        break;
                    case QuestState.Start:
                        _messagePanelViewHandler.AcceptButton.onClick.AddListener(() => AcceptTask(ID, selfID));
                        _message =
                            $"{_questConfig.StartMessage} wood {_buildingConfig.WoodCost}, stone {_buildingConfig.FoodCost}, food {_buildingConfig.FoodCost}";
                        ChangeMessage(_message, _questConfig.AcceptTaskButtonText, true);
                        _state = QuestState.Wait;
                        break;
                    case QuestState.Wait:

                        break;
                    case QuestState.Check:
                        if (Check())
                        {
                            ChangeMessage(_questConfig.GETResMessage, _questConfig.BuildButtonText, true);
                        }
                        else
                        {
                            ChangeMessage(_message, _questConfig.BuildButtonText, false);
                        }

                        break;
                    case QuestState.Done:
                        _messagePanelViewHandler.ShowMessage(_questConfig.DoneMessage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ChangeMessage(string message, string buttonName, bool value)
        {
            _messagePanelViewHandler.ChangeTextOnButton(buttonName);
            _messagePanelViewHandler.AcceptButton.gameObject.SetActive(value);
            _messagePanelViewHandler.AcceptButton.interactable = value;
            _messagePanelViewHandler.ShowMessage(message);
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
            
            _messagePanelViewHandler.AcceptButton.interactable = false;
            _messagePanelViewHandler.AcceptButton.gameObject.SetActive(false);
            _messagePanelViewHandler.AcceptButton.onClick.RemoveListener(()=>Build(ID, selfID));
            
            _state = QuestState.Done;

            OnStateChange?.Invoke(_state);
            OnQuestDone?.Invoke(_npcID);
            StartDialog(ID, selfID);
        }

        private void AcceptTask(int ID, int selfID)
        {

            if (!_isAccepted)
            {
                _messagePanelViewHandler.AcceptButton.interactable = false;
                _messagePanelViewHandler.AcceptButton.onClick.RemoveListener(() => AcceptTask(ID, selfID));
                
                _messagePanelViewHandler.AcceptButton.gameObject.SetActive(false);
                _messagePanelViewHandler.ActivationPanel(false);
                var questMessage =
                    $"{_questConfig.QuestInfo}\nwood {_buildingConfig.WoodCost}\nstone {_buildingConfig.FoodCost}\nfood {_buildingConfig.FoodCost}";
                OnQuestStart?.Invoke(_questConfig.QuestName, questMessage, _npcID);

                _state = QuestState.Check;
                _isAccepted = true;

                _messagePanelViewHandler.AcceptButton.onClick.AddListener(()=>Build(ID, selfID));
                _messagePanelViewHandler.ChangeTextOnButton(_questConfig.BuildButtonText);
                _message =
                    $"{_questConfig.WaitingMessage}\nwood {_buildingConfig.WoodCost}\nstone {_buildingConfig.FoodCost}\nfood {_buildingConfig.FoodCost}";
                _messagePanelViewHandler.AcceptButton.gameObject.SetActive(false);
                
                OnStateChange?.Invoke(_state);
                StartDialog(ID, selfID);
            }
        }

        public void Cleanup()
        {
            _npc.HitHandler.OnHitEnter -= StartDialog;
            _npc.HitHandler.OnHitExit -= EndDialog;
        }
    }
}
