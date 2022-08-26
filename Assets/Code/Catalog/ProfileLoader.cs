using Code.Controllers;
using Code.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Catalog
{
    public class ProfileLoader : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _gameRoomsButton;
        [SerializeField] private Button _profileButton;
        [SerializeField] private GameObject _gameRoomsPanel;
        [SerializeField] private GameObject _profilePanel;
        [SerializeField] private TMP_Text _loadingText;
        [Space(20)] 
        [SerializeField] private PlayerNamePanelView _enterNamePanel;
        [SerializeField] private Transform _charactersPanel;
        [SerializeField] private Transform _otherPanel;
        [SerializeField] private LineElementView _characterAllInfo;
        [SerializeField] private LineElementView _lineElement;
        [SerializeField] private TextElementView _gold;
        [SerializeField] private TextElementView _experience;

        private void Awake()
        {
            var system = new SystemController();
            _exitButton.onClick.AddListener(system.Quit);

            _gameRoomsButton.onClick.AddListener(GoToRoomsList);
            _profileButton.onClick.AddListener(GoToProfile);

            GoToRoomsList();

            var catalogManager =
                new CatalogManager(_charactersPanel, _otherPanel, _enterNamePanel, _gold, _experience, _lineElement, _characterAllInfo);
        }

        private void GoToRoomsList()
        {
            _profilePanel.SetActive(false);
            _gameRoomsPanel.SetActive(true);
            _loadingText.gameObject.SetActive(true);
        }

        private void GoToProfile()
        {
            _gameRoomsPanel.SetActive(false);
            _profilePanel.SetActive(true);
            _loadingText.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _exitButton.onClick.RemoveAllListeners();
            _gameRoomsButton.onClick.RemoveAllListeners();
            _profileButton.onClick.RemoveAllListeners();
        }
    }
}
