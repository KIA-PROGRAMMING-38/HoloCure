using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public event Action OnHoverForOtherButton;
    public event Action<MyButton> OnHoverForController;
    public event Action OnClick;
    public event Action<MyButton> OnClickForController;
    [SerializeField] private GameObject _defaultFrame;
    [SerializeField] private GameObject _hoveredFrame;
    public void ActivateHoveredFrame()
    {
        OnHoverForOtherButton?.Invoke();
        _defaultFrame.SetActive(false);
        _hoveredFrame.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ActivateHoveredFrame();
        OnHoverForController?.Invoke(this);        
    }
    public void DeActivateHoveredFrame()
    {
        _defaultFrame.SetActive(true);
        _hoveredFrame.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        OnClick?.Invoke();
        OnClickForController?.Invoke(this);
    }
}
