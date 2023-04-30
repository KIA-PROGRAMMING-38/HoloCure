using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyFlashButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public event Action<MyFlashButton> OnHoverForController;
    public event Action OnClick;
    public event Action<MyFlashButton> OnClickForController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverForController?.Invoke(this);
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
