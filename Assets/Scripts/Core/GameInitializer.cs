using Battle;
using UnityEngine;
using Character;
using UI;
using Util;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private Player _player;
        [SerializeField] private CharacterSpawner _spawner;

        private void Start()
        {
            _player.Initialize();
            _spawner.Initialize();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _spawner.SpawnEnemy(Enemy.EEnemyType.Default, Vector3.zero);
            }
        }
#endif
    }
}
