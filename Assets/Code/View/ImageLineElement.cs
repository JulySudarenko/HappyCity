using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public class ImageLineElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _description;

        public Image Icon => _icon;

        public TMP_Text Description => _description;
    }
}
