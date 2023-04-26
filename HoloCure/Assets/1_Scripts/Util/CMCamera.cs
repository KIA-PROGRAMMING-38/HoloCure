using Cinemachine;
using System.Collections;
using UnityEngine;

namespace Util
{
    public class CMCamera : MonoBehaviour
    {
        private static CMCamera _instance;

        [SerializeField] private CinemachineVirtualCamera _myCamera;
        private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

        private void Awake()
        {
            _instance = this;
            _cinemachineBasicMultiChannelPerlin = _myCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _shakeCoroutine = ShakeCoroutine();
        }
        public static void SetCameraFollow(Transform transform) => _instance._myCamera.Follow = transform;

        private static float _duration;
        private static float _intensity;
        public static void Shake(float duration = 0.1f, float intensity = 2f)
        {
            _duration = duration;
            _intensity = intensity;
            _instance.StartCoroutine(_instance._shakeCoroutine);
        }
        private IEnumerator _shakeCoroutine;
        private IEnumerator ShakeCoroutine()
        {
            while (true)
            {
                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;

                yield return TimeStore.GetWaitForSeconds(_duration);

                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;

                StopCoroutine(_shakeCoroutine);

                yield return null;
            }
        }
    }
}