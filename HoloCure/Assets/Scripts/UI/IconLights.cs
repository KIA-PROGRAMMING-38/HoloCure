using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class IconLights : MonoBehaviour
{
    private RectTransform[] _transforms;
    private const int DISTANCE = 100;
    private void Awake()
    {
        _transforms = GetComponentsInChildren<RectTransform>();

        int angleStep = 360 / (_transforms.Length - 1);

        for (int i = 1; i < _transforms.Length; i++)
        {
            float angleRadians = angleStep * i * Mathf.Deg2Rad;

            float x = _transforms[0].rect.center.x + DISTANCE * Mathf.Cos(angleRadians);
            float y = _transforms[0].rect.center.y + DISTANCE * Mathf.Sin(angleRadians);

            _transforms[i].anchoredPosition = new Vector2(x, y);

            _transforms[i].localRotation = Quaternion.Euler(0, 0, -90 + angleStep * i);
        }
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(Rotate);
    }

    private void Rotate(Unit unit)
    {
        _transforms[0].Rotate(Vector3.forward, 0.5f);
    }
}
