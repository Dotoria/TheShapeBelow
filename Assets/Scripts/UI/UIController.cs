using UnityEngine;
using Util;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject _startUI;
        [SerializeField] private GameObject _endUI;
        [SerializeField] private JoystickUI _joystickUI;

        public void Initialize()
        {
            _startUI.SetActive(true);
            _endUI.SetActive(false);
            _joystickUI.Initialize();
        }

        public void StartGame()
        {
            InputManager.BlockInput(false);
            _startUI.SetActive(false);
        }
    }
}