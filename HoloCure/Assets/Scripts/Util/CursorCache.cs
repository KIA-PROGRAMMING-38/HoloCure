using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Util
{
    public class CursorCache : MonoBehaviour
    {
        public static Vector2 CursorScreenPosition => _cursorScreenPosition;
        private static Vector2 _cursorScreenPosition;
        public static Vector2 CursorWorldPosition => _cursorWorldPosition;
        private static Vector2 _cursorWorldPosition;

        private static Camera _mainCamera;
        private void Awake() => _mainCamera = Camera.main;

        private void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(SetCursorPosition);
        }

        private static void SetCursorPosition(Unit unit)
        {
            _cursorScreenPosition = Input.mousePosition;
            _cursorWorldPosition = _mainCamera.ScreenToWorldPoint(_cursorScreenPosition);
        }

        public static float GetAngleToCursor(Vector2 position)
        {
            Vector2 direction = _cursorWorldPosition - position;

            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public static Vector2 GetDirectionToCursor(Vector2 position) => (_cursorWorldPosition - position).normalized;

        public static void SetCameraRotationDefault() => _mainCamera.transform.rotation = default;
    }
}