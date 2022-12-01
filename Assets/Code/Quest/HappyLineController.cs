using System;
using System.Collections.Generic;
using Code.Interfaces;
using Code.Timer;

namespace Code.Quest
{
    internal class HappyLineController : IExecute
    {
        public Action<int> StartNewQuest;
        private ITimeRemaining _questTimer;
        private ITimeRemaining _removeTimer;
        private int _generalHappyLine;
        private readonly List<int> _happinessList = new List<int>();
        private float _addTime = 0;
        private float _removeTime = 0;
        private bool _isWaiting;

        public HappyLineController()
        {
            _happinessList.Add(75);
            _happinessList.Add(75);
            _happinessList.Add(75);
            //GetGeneralHappy();
        }

        public int Population => _happinessList.Count;

        public void ChangeCurrentPopulation(int happy)
        {
            _happinessList.Add(happy);
        }

        public void ChangeHappiness(int start, int delta)
        {
            for (int i = 0; i < _happinessList.Count; i++)
            {
                if (_happinessList[i] == start)
                {
                    _happinessList[i] += delta;
                    GetGeneralHappy();
                    return;
                }
            }
        }

        private void GetGeneralHappy()
        {
            var sumHappy = 0;

            for (int i = 0; i < _happinessList.Count; i++)
            {
                sumHappy += _happinessList[i];
            }

            _generalHappyLine = sumHappy / _happinessList.Count;
            CheckTimer();
        }

        private void CheckTimer()
        {
            if (_generalHappyLine > 90)
            {
                _addTime = 2.0f;
                _removeTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 80)
            {
                _addTime = 3.0f;
                _removeTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 70)
            {
                _addTime = 4.0f;
                _removeTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 60)
            {
                _addTime = 5.0f;
                _removeTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 50)
            {
                _addTime = 0.0f;
                _removeTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 40)
            {
                _removeTime = 5.0f;
                _addTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 30)
            {
                _removeTime = 4.0f;
                _addTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 20)
            {
                _removeTime = 3.0f;
                _addTime = 0.0f;
                return;
            }

            if (_generalHappyLine > 10)
            {
                _removeTime = 2.0f;
                _addTime = 0.0f;
            }
        }

        public void Execute(float deltaTime)
        {
            if (_addTime > 0 && !_isWaiting)
            {
                _questTimer = new TimeRemaining(StartQuest, _addTime, false);
                _questTimer.AddTimeRemaining();
                _isWaiting = true;
            }

            if (_removeTime > 0 && !_isWaiting)
            {
                _removeTimer = new TimeRemaining(ReducePopulation, _removeTime, false);
                _removeTimer.AddTimeRemaining();
                _isWaiting = true;
            }
        }

        private void StartQuest()
        {
            StartNewQuest?.Invoke(_happinessList.Count);
            _questTimer.RemoveTimeRemaining();
            GetGeneralHappy();
            _isWaiting = false;
        }

        private void ReducePopulation()
        {
            _happinessList.Remove(_happinessList[0]);

            _removeTimer.RemoveTimeRemaining();
            GetGeneralHappy();
            _isWaiting = false;
        }
    }
}
