using Code.Controllers;
using Code.View;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Catalog
{
    public class ProfileLoader : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [Space(20)] 
        [SerializeField] private PlayerNamePanelView _enterNamePanel;
        [SerializeField] private Transform _charactersPanel;
        [SerializeField] private Transform _infoPanel;
        [SerializeField] private LineElementView _characterAllInfo;
        [SerializeField] private LineElementView _lineElement;
        [SerializeField] private TextElementView _gold;
        [SerializeField] private TextElementView _experience;

        private void Awake()
        {
            var system = new SystemController();
            _exitButton.onClick.AddListener(system.Quit);

            var catalogManager =
                new CatalogManager(_charactersPanel, _infoPanel, _enterNamePanel, _gold, _experience, _lineElement, _characterAllInfo);
        }
        private void OnDestroy()
        {
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}
