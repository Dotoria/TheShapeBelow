using System;
using UnityEngine;

namespace Util
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;
        
        public static Action<bool> OnDragging;
        public static Vector2 XY;
        
        private Vector3 _initPosition;
        private bool _isBlocked;

        private const float DRAG_RANGE = 5f;

        private static InputManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    GameObject go = new GameObject("InputManager");
                    _instance = go.AddComponent<InputManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private void Update()
        {
            Debug.Log($"Update: {_isBlocked}");
            if (_isBlocked)
                return;

#if UNITY_EDITOR
            Vector2 screenPos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchBegan(screenPos);
            }
            else if (Input.GetMouseButton(0))
            {
                OnTouchMoved(screenPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnTouchEnded(screenPos);
            }
#else
            if (Input.touchCount <= 0)
                return;
            
            var input = Input.GetTouch(0);
            Vector2 screenPos = input.position;

            if (input.phase == TouchPhase.Began)
            {
                OnTouchBegan(screenPos);
            }
            else if (input.phase == TouchPhase.Moved || input.phase == TouchPhase.Stationary)
            {
                OnTouchMoved(screenPos);
            }
            else if (input.phase == TouchPhase.Ended || input.phas == TouchPhase.Canceled)
            {
                OnTouchEnded(screenPos);
            }
#endif
        }

        private void OnTouchBegan(Vector2 screenPos)
        {
            _initPosition = screenPos;
            XY = screenPos;
            OnDragging?.Invoke(true);
        }

        private void OnTouchMoved(Vector2 screenPos)
        {
            Vector2 direction = screenPos - (Vector2)_initPosition;
            float dist = direction.magnitude;

            if (dist > DRAG_RANGE)
            {
                XY = (Vector2)_initPosition + direction.normalized * DRAG_RANGE;
            }
            else
            {
                XY = screenPos;
            }
        }

        private void OnTouchEnded(Vector2 screenPos)
        {
            OnDragging?.Invoke(false);
        }
        
        public static void BlockInput(bool isBlocked) => Instance._isBlocked = isBlocked;
    }
}