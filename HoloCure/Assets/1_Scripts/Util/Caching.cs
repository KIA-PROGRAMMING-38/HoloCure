using Cinemachine;
using UnityEngine;

namespace Util
{
    public class Caching : MonoBehaviour
    {
        public static Vector2 MouseScreenPos => _mouseScreenPos;
        private static Vector2 _mouseScreenPos;
        public static Vector2 MouseWorldPos => _mouseWorldPos;
        private static Vector2 _mouseWorldPos;
        public static Vector2 CenterScreenPos => _centerScreenPos;
        private static Vector2 _centerScreenPos;
        public static Vector2 CenterWorldPos => _centerWorldPos;
        private static Vector2 _centerWorldPos;

        private static Camera _mainCamera;
        private void Awake()
        {
            _mainCamera = Camera.main;
            _centerScreenPos = new Vector2(Screen.width / 2, Screen.height / 2);
        }
        private void Update()
        {
            _mouseScreenPos = Input.mousePosition;
            _mouseWorldPos = _mainCamera.ScreenToWorldPoint(_mouseScreenPos);
            _centerWorldPos = _mainCamera.ScreenToWorldPoint(_centerScreenPos);
        }
        public static float GetAngleToMouse(Vector2 position)
        {
            Vector2 direction = _mouseWorldPos - position;

            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}