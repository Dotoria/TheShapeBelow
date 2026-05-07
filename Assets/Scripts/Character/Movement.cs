using UnityEngine;

namespace Character
{
    public class Movement
    {
        private readonly Transform _transform;
        private readonly float _moveSpeed;
        private readonly float _smoothTime;
        private readonly float _turnSpeed;

        private Vector2 _velocity;
        private Vector2 _currentVelocityRef;

        public Movement(Transform transform, float moveSpeed, float smoothTime, float turnSpeed)
        {
            _transform = transform;
            _moveSpeed = moveSpeed;
            _smoothTime = smoothTime;
            _turnSpeed = turnSpeed;
        }

        public void Move(Vector2 direction)
        {
            Vector2 inputDir = direction.sqrMagnitude > 0.001f
                ? direction.normalized
                : Vector2.zero;

            Vector2 targetVelocity = inputDir * _moveSpeed;

            _velocity = Vector2.SmoothDamp(
                _velocity,
                targetVelocity,
                ref _currentVelocityRef,
                _smoothTime
            );

            _transform.position += (Vector3)_velocity * Time.deltaTime;

            if (_velocity.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(Vector3.forward, _velocity.normalized);

                _transform.rotation = Quaternion.Lerp(
                    _transform.rotation,
                    targetRotation,
                    _turnSpeed * Time.deltaTime
                );
            }
        }
    }
}