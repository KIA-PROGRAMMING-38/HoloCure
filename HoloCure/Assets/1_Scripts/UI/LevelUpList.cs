using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpList : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public event Action OnHoverForOtherList;
    public event Action<LevelUpList> OnClickForController;

    [SerializeField] private GameObject _defaultFrame;
    [SerializeField] private GameObject _hoveredFrame;
    [SerializeField] private GameObject _selectCursor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverForOtherList?.Invoke();
        _defaultFrame.SetActive(false);
        _hoveredFrame.SetActive(true);
        _selectCursor.SetActive(true);
    }
    public void ActivateDefaultFrame()
    {
        _defaultFrame.SetActive(true);
        _hoveredFrame.SetActive(false);
        _selectCursor.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickForController?.Invoke(this);
    }
}
