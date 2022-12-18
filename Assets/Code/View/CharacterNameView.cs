using Photon.Pun;
using UnityEngine;

namespace Code.View
{
    public class CharacterNameView : MonoBehaviourPun
    {
        [SerializeField] private TextNameView _tmpTextNameView;

        private void Awake()
        {
            _tmpTextNameView.TextName.text = photonView.Owner.NickName;
        }
    }
}
