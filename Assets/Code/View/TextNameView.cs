using TMPro;
using UnityEngine;

namespace Code.View
{
    public class TextNameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Vector3 _screenOffset = new Vector3(0f, 30f, 0f);
        [SerializeField] private Transform _target;

        private Camera _camera;

        public TMP_Text TextName => _name;

        private void Start()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            _camera = Camera.main;
            transform.SetParent(canvas.transform, false);
        }

        
        private void LateUpdate()
        {
            if (_target != null)
            {
                transform.position = _camera.WorldToScreenPoint(_target.position) + _screenOffset;
            }
            else
            {
                Destroy(_name.gameObject);
                Destroy(this);
            }
        }
    }
}
