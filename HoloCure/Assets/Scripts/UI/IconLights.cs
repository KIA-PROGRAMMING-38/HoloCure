using UnityEngine;

public class IconLights : MonoBehaviour
{
    private RectTransform _icon;
    [SerializeField] private RectTransform[] _lights;
    private void Awake() => _icon = GetComponent<RectTransform>();
    private const int DISTANCE = 100;
    private void Start()
    {
        int angleStep = 360 / _lights.Length;

        for (int i = 0; i < _lights.Length; i++)
        {
            float angleRadians = angleStep * i * Mathf.Deg2Rad;

            float x = _icon.rect.center.x + DISTANCE * Mathf.Cos(angleRadians);
            float y = _icon.rect.center.y + DISTANCE * Mathf.Sin(angleRadians);

            _lights[i].anchoredPosition = new Vector2(x, y);

            _lights[i].localRotation = Quaternion.Euler(0, 0, -90 + angleStep * i);
        }
    }
    private void Update()
    {
        _icon.Rotate(Vector3.forward, 0.5f);
    }
}
