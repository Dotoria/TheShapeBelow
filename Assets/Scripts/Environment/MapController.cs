using UnityEngine;

namespace Environment
{
    public class MapController : MonoBehaviour
    {
        private static MapController _instance;

        [SerializeField] private Renderer _backgroundRenderer;

        [Header("Flow")]
        [SerializeField] private float _speedMultiplier = 0.1f;
        [SerializeField] private float _scrollSpeedMultiplier = 0.5f;
        [SerializeField] private float _flowSmoothTime = 0.5f;

        [Header("Player Move Area")]
        [SerializeField] private Vector2 _minMoveArea = new Vector2(-3f, -3f);
        [SerializeField] private Vector2 _maxMoveArea = new Vector2(3f, 3f);

        private Material _material;
        private Vector2 _flowOffset;
        private Vector2 _flowVelocity;

        public void Initialize()
        {
            if (null == _instance)
                _instance = this;

            _material = _backgroundRenderer.material;
        }

        public static void MoveBackgroundStatic(Vector2 inputDirection, bool isBlockedByBoundary)
            => _instance.MoveBackground(inputDirection, isBlockedByBoundary);

        private void MoveBackground(Vector2 inputDirection, bool isBlockedByBoundary)
        {
            float multiplier = isBlockedByBoundary
                ? _scrollSpeedMultiplier
                : _speedMultiplier;

            Vector2 targetFlow = -inputDirection * multiplier;

            _flowVelocity = Vector2.Lerp(
                _flowVelocity,
                targetFlow,
                Time.deltaTime / Mathf.Max(_flowSmoothTime, 0.001f)
            );

            _flowOffset += _flowVelocity * Time.deltaTime;
            _material.SetVector("_FlowOffset", new Vector4(_flowOffset.x, _flowOffset.y, 0f, 0f));
        }
        
        public static Vector2 GetAllowedMoveDirection(Vector3 playerPosition, Vector2 inputDirection, out bool isBlocked)
            => _instance.GetAllowedDirection(playerPosition, inputDirection, out isBlocked);
        
        private Vector2 GetAllowedDirection(Vector3 playerPosition, Vector2 inputDirection, out bool isBlocked)
        {
            isBlocked = false;

            Vector2 allowedDirection = inputDirection;

            if (inputDirection.x > 0f && playerPosition.x >= _maxMoveArea.x)
            {
                allowedDirection.x = 0f;
                isBlocked = true;
            }
            else if (inputDirection.x < 0f && playerPosition.x <= _minMoveArea.x)
            {
                allowedDirection.x = 0f;
                isBlocked = true;
            }

            if (inputDirection.y > 0f && playerPosition.y >= _maxMoveArea.y)
            {
                allowedDirection.y = 0f;
                isBlocked = true;
            }
            else if (inputDirection.y < 0f && playerPosition.y <= _minMoveArea.y)
            {
                allowedDirection.y = 0f;
                isBlocked = true;
            }

            if (allowedDirection.sqrMagnitude > 0.001f)
                allowedDirection.Normalize();

            return allowedDirection;
        }
    }
}