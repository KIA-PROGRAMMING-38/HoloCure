using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private int _order = -20;
    private Stack<UIPopup> _popupStack = new Stack<UIPopup>();
    private GameObject _root;
    public void Init()
    {
        _root = new GameObject("UI Root");
    }
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order += 1;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
    private static readonly Vector3 s_defaultScale = Vector3.one;
    public T OpenPopupUI<T>(Transform parent = null) where T : UIPopup
    {
        GameObject go = Managers.Resource.Instantiate(typeof(T).Name);
        T popup = go.GetComponent<T>();
        _popupStack.Push(popup);

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }
        else
        {
            go.transform.SetParent(_root.transform);
        }

        go.transform.localScale = s_defaultScale;

        return popup;
    }
    public void ClosePopupUI(UIPopup popup)
    {
        if (_popupStack.Count == 0)
        {
            Debug.Assert(false, $"Failed to close PopupUI: {popup}. Popup Stack is empty.");
            return;
        }

        if (_popupStack.Peek() != popup)
        {
            Debug.Assert(false, $"Failed to close PopupUI: {popup}. Popup on top of the stack is {_popupStack.Peek()}.");
            return;
        }

        ClosePopupUI();
    }
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
        {
            Debug.Assert(false, $"Failed to close PopupUI. Popup Stack is empty.");
            return;
        }

        UIPopup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        _order -= 1;
    }
}