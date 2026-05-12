using UnityEngine;

namespace Onboarding
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _arrowWidth = 0.5f;

        [SerializeField] private float _arrowSpacing = 1.5f;
        [SerializeField] private float _scrollSpeed = 1f;

        private Material _material;
        private Transform _start;
        private Transform _end;

        public void Initialize()
        {
            // _lineRenderer.positionCount = 2;
            _material = _lineRenderer.material;
            _lineRenderer.widthMultiplier = _arrowWidth;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (null == _start || null == _end)
                return;

            Vector3 startPos = _start.position;
            Vector3 endPos = _end.position;

            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);

            float distance = Vector3.Distance(startPos, endPos);

            float tileCount = distance / _arrowSpacing;

            _material.mainTextureScale = new Vector2(tileCount, 1f);
            _material.mainTextureOffset += new Vector2(Time.deltaTime * _scrollSpeed, 0f);
        }
        
        public void SetPositions(Transform start, Transform end)
        {
            _start = start;
            _end = end;
        }
    }
}