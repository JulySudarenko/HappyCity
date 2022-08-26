using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.View
{
    public sealed class LineElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textUp;
        [SerializeField] private TMP_Text _textDown;
        [SerializeField] private Button _button;

        public Button Button => _button;

        public TMP_Text TextUp => _textUp;

        public TMP_Text TextDown => _textDown;
    }
}
