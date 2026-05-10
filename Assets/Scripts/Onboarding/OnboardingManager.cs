using UnityEngine;

namespace Onboarding
{
    public class OnboardingManager : MonoBehaviour
    {
        private static OnboardingManager _instance;
        
        private Arrow _arrow;
        
        public static void Initialize(Arrow arrow)
        {
            if (null == _instance)
            {
                GameObject go = new GameObject("OnboardingManager");
                _instance = go.AddComponent<OnboardingManager>();
                DontDestroyOnLoad(go);
            }

            _instance._arrow = arrow;
            _instance._arrow.Initialize();
        }
        
        public static void ShowArrow(Transform from, Transform to)
        {
            _instance._arrow.SetPositions(from, to);
            _instance._arrow.gameObject.SetActive(true);
        }
        
        public static void HideArrow()
        {
            _instance._arrow.gameObject.SetActive(false);
            _instance._arrow.SetPositions(null, null);
        }
    }
}