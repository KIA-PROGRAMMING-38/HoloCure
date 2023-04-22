using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public event Action OnHoverForOtherButton;
    public event Action<MyButton> OnHoverForController;
    public event Action<MyButton> OnClickForController;
    [SerializeField] private GameObject _defaultFrame;
    [SerializeField] private GameObject _hoveredFrame;
    public void HoveredByKey()
    {
        OnHoverForOtherButton?.Invoke();
        _hoveredFrame.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverForOtherButton?.Invoke();
        OnHoverForController?.Invoke(this);
        _hoveredFrame.SetActive(true);
    }
    public void DeActivateHoveredFrame() => _hoveredFrame.SetActive(false);
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickForController?.Invoke(this);
    }
}
