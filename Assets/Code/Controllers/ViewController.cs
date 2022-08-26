using Code.Factory;
using Code.Interfaces;
using Code.View;
using Code.ViewHandlers;
using UnityEngine;

namespace Code.Controllers
{
    internal class ViewController : IInitialization, ICleanup
    {
        
        private readonly CharacterSelectedPanelViewHandler _characterSelectedPanel;

        public ViewController(Transform characterSelectedPanel, LineElementView lineElement, CharacterSpawnHandler spawnHandler)
        {
            _characterSelectedPanel = new CharacterSelectedPanelViewHandler(characterSelectedPanel, lineElement, spawnHandler);
        }

        public void Initialize()
        {
            _characterSelectedPanel.Init();
        }

        public void Cleanup()
        {
            _characterSelectedPanel.Cleanup();
        }
    }
}
