using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Util
{
    public class CursurCache : MonoBehaviour
    {
        public static Vector2 MouseScreenPos => _mouseScreenPos;
        private static Vector2 _mouseScreenPos;
        public static Vector2 MouseWorldPos => _mouseWorldPos;
        private static Vector2 _mouseWorldPos;

        private static Camera _mainCamera;
        private void Awake() => _mainCamera = Camera.main;
        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(GetMousePosition);
        }
        private static void GetMousePosition(Unit unit)
        {
            _mouseScreenPos = Input.mousePosition;
            _mouseWorldPos = _mainCamera.ScreenToWorldPoint(_mouseScreenPos);
        }
        public static float GetAngleToMouse(Vector2 position)
        {
            Vector2 direction = _mouseWorldPos - position;

            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        public static void SetCameraRotationDefault() => _mainCamera.transform.rotation = default;
    }
}