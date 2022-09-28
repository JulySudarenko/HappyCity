using System;
using System.Collections.Generic;
using Code.Login;
using Code.View;

namespace Code.Factory
{
    internal class PlayersList
    {
        private readonly List<CharacterPhotonView> _charactersList;
        private readonly LoadingIndicatorView _loadingIndicator;

        public PlayersList(LoadingIndicatorView loadingIndicator)
        {
            _charactersList = new List<CharacterPhotonView>();
            _loadingIndicator = loadingIndicator;
        }

        public CharacterPhotonView[] CharactersList => _charactersList.ToArray();

        public void AddPlayer(CharacterPhotonView character)
        {
            _charactersList.Add(character);
            _loadingIndicator.UpdateFeedbackText($"Player {character.photonView.Owner} is connected");
        }
    }
}
