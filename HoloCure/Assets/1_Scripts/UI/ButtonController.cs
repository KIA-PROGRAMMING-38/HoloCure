using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButtonUI : MonoBehaviour, IPointerEnterHandler
{
    public event Action<float> OnMouseCursorEnter;
    private RectTransform _transform;
    private void Awake() => _transform = GetComponent<RectTransform>();
    public void OnPointerEnter(PointerEventData eventData) => OnMouseCursorEnter?.Invoke(_transform.position.y);
}
