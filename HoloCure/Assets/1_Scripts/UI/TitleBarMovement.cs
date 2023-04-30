using UnityEngine;

public class TitleBarMovement : MonoBehaviour
{
    private RectTransform _rectTransform;
    private void Awake() => _rectTransform = GetComponent<RectTransform>();
    private readonly Vector2 MOVE_SPEED = new(-0.5f, 0);
    private readonly Vector2 MOVE_POINT = new(-40, 0);
    private void Update()
    {
        _rectTransform.anchoredPosition += MOVE_SPEED;
        if (_rectTransform.anchoredPosition == MOVE_POINT)
        {
            _rectTransform.anchoredPosition = default;
        }
    }
}
