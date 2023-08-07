using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static readonly Vector3 DEFAULT_SCALE = Vector3.one;

    private int _currentCanvasOrder = -20;

    private Stack<UIPopup> _popupStack;

    private GameObject _root;

    public void Init()
    {
        _popupStack = new Stack<UIPopup>();
        _root = new GameObject("UI Root");
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetComponentAssert<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _currentCanvasOrder;
            _currentCanvasOrder += 1;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T OpenPopup<T>(Transform parent = null) where T : UIPopup
    {
        T popup = SetupUI<T>(parent);

        _popupStack.Push(popup);

        return popup;
    }

    public T OpenSubItem<T>(Transform parent = null) where T : UISubItem
    {
        return SetupUI<T>(parent);
    }

    private T SetupUI<T>(Transform parent = null) where T : UIBase
    {
        GameObject prefab = Managers.Resource.LoadPrefab(typeof(T).Name);
        GameObject go = Managers.Resource.Instantiate(prefab);

        if (parent != null)
        {
            go.transform.SetParent(parent);
        }
        else
        {
            go.transform.SetParent(_root.transform);
        }

        go.transform.localScale = DEFAULT_SCALE;
        go.transform.localPosition = prefab.transform.position;

        return go.GetComponentAssert<T>();
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
        _currentCanvasOrder -= 1;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
}