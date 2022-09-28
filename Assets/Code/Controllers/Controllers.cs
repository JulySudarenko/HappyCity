using System.Collections.Generic;
using Code.Interfaces;

namespace Code.Controllers
{
    public sealed class Controllers : IInitialization, IFixedExecute, IExecute, ICleanup
    {
        private readonly List<IInitialization> _initializeControllers;
        private readonly List<IFixedExecute> _fixedControllers;
        private readonly List<IExecute> _executeControllers;
        private readonly List<ILateExecute> _lateExecuteControllers;
        private readonly List<ICleanup> _cleanupControllers;

        internal Controllers()
        {
            _initializeControllers = new List<IInitialization>();
            _fixedControllers = new List<IFixedExecute>();
            _executeControllers = new List<IExecute>();
            _lateExecuteControllers = new List<ILateExecute>();
            _cleanupControllers = new List<ICleanup>();
        }

        internal Controllers Add(IController controller)
        {
            if (controller is IInitialization initializeController)
            {
                _initializeControllers.Add(initializeController);
            }

            if (controller is IFixedExecute fixedController)
            {
                _fixedControllers.Add(fixedController);
            }

            if (controller is IExecute executeController)
            {
                _executeControllers.Add(executeController);
            }

            if (controller is ILateExecute lateController)
            {
                _lateExecuteControllers.Add(lateController);
            }

            if (controller is ICleanup cleanupController)
            {
                _cleanupControllers.Add(cleanupController);
            }
            
            return this;
        }

        public void Initialize()
        {
            for (var index = 0; index < _initializeControllers.Count; ++index)
            {
                _initializeControllers[index].Initialize();
            }
        }

        public void FixedExecute(float deltaTime)
        {
            for (var index = 0; index < _fixedControllers.Count; ++index)
            {
                _fixedControllers[index].FixedExecute(deltaTime);
            }
        }

        public void Execute(float deltaTime)
        {
            for (var index = 0; index < _executeControllers.Count; ++index)
            {
                _executeControllers[index].Execute(deltaTime);
            }
        }
        
        public void LateExecute(float deltaTime)
        {
            for (var index = 0; index < _lateExecuteControllers.Count; ++index)
            {
                _lateExecuteControllers[index].LateExecute(deltaTime);
            }
        }

        public void Cleanup()
        {
            for (var index = 0; index < _cleanupControllers.Count; ++index)
            {
                _cleanupControllers[index].Cleanup();
            }
        }
    }
}
