using TMPro;
using UnityEngine;

namespace Code.View
{
    public class TextElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void ShowCurrency(string currency, string amount)
        {
            _text.text = $"{currency} : {amount}";
        }
    }
}
