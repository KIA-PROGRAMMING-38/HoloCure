using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : UIBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private GameObject _defaultFrame;
    private GameObject _hoveredFrame;
    protected override void Awake()
    {
        _defaultFrame = transform.FindAssert("Default Frame").gameObject;
        _hoveredFrame = transform.FindAssert("Hovered Frame").gameObject;
    }
    public void ActivateHoveredFrame()
    {
        _defaultFrame.SetActive(false);
        _hoveredFrame.SetActive(true);
    }
    public void DeActivateHoveredFrame()
    {
        _defaultFrame.SetActive(true);
        _hoveredFrame.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
