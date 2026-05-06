using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class JoystickUI : MonoBehaviour
    {
        [SerializeField] private Image _outerCircle;
        [SerializeField] private Image _innerCircle;

        private void Start()
        {
            InputManager.BlockInput(false);
            InputManager.OnDragging += HandleDragging;
        }
        
        private void HandleDragging(bool isDragging)
        {
            Debug.Log($"Drag: {isDragging}");
            _outerCircle.gameObject.SetActive(isDragging);
            _innerCircle.gameObject.SetActive(isDragging);

            if (isDragging)
            {
                _outerCircle.transform.position = InputManager.XY;
                _innerCircle.transform.localPosition = Vector2.zero;
            }
        }

        private void Update()
        {
            if (_outerCircle.gameObject.activeSelf)
            {
                _innerCircle.transform.position = InputManager.XY;
                Vector2 delta = _innerCircle.rectTransform.anchoredPosition;
            }
        }
    }
}