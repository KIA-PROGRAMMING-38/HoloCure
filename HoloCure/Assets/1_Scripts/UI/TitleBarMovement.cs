using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class TitleBarMovement : MonoBehaviour
{
    private RectTransform _rectTransform;
    private readonly Vector2 MOVE_SPEED = new Vector2(-0.5f, 0);
    private readonly Vector2 MOVE_POINT = new Vector2(-40, 0);
    private void Awake() => _rectTransform = gameObject.GetComponent<RectTransform>();
    private void Start() => this.UpdateAsObservable().Subscribe(Move);
    private void Move(Unit unit)
    {
        _rectTransform.anchoredPosition += MOVE_SPEED;
        if (_rectTransform.anchoredPosition == MOVE_POINT)
        {
            _rectTransform.anchoredPosition = default;
        }
    }
}
