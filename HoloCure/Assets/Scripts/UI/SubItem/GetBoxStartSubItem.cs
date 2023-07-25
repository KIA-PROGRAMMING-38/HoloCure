using StringLiterals;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetBoxStartSubItem : UISubItem
{
    #region Enums

    enum Buttons
    {
        OpenButton
    }

    enum Images
    {
        OpenButton
    }

    enum Texts
    {
        OpenText
    }

    #endregion

    #region UI Fields and Properties

    private Buttons _currentButton;
    private Buttons CurrentButton
    {
        get => _currentButton;
        set
        {
            SetButtonNormal(_currentButton);
            SetButtonHighlighted(value);

            _currentButton = value;
        }
    }

    private static readonly Color s_normalColor = Color.white;
    private static readonly Color s_highlightedColor = Color.black;

    #endregion

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        CurrentButton = _currentButton;

        foreach (Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            Button button = GetButton((int)buttonIndex);
            button.BindViewEvent(OnEnterButton, ViewEvent.Enter, this);
            button.BindViewEvent(OnClickButton, ViewEvent.Click, this);
        }

        this.UpdateAsObservable()
            .Subscribe(OnPressKey);

        Managers.Sound.Play(SoundID.BoxOpenStart);
    }

    public void InitButton(int buttonIndex)
    {
        _currentButton = (Buttons)buttonIndex;
    }

    #region Event Handlers

    private void OnEnterButton(PointerEventData eventData)
    {
        Buttons nextButton = Enum.Parse<Buttons>(eventData.pointerEnter.name);

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.ButtonMove);
    }

    private void OnClickButton(PointerEventData eventData)
    {
        Buttons button = Enum.Parse<Buttons>(eventData.pointerClick.name);

        ProcessButton(button);
    }

    private void OnPressKey(Unit unit)
    {
        if (Input.GetButtonDown(InputLiteral.CONFIRM))
        {
            ProcessButton(CurrentButton);
        }
    }

    #endregion

    #region UI Appearance Update

    private void SetButtonNormal(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_Button_0");
        GetText((int)buttonIndex).color = s_normalColor;
    }

    private void SetButtonHighlighted(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_Button_1");
        GetText((int)buttonIndex).color = s_highlightedColor;
    }

    #endregion

    #region Button Actions

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.OpenButton: OnClickOpenButton(); break;
            default: throw new ArgumentOutOfRangeException(nameof(button));
        }

        Managers.Sound.Play(SoundID.ButtonClick);
    }

    private void OnClickOpenButton()
    {
        Managers.UI.OpenSubItem<GetBoxOngoingSubItem>(transform.parent);
        
        CloseSubItem();
    }

    #endregion
}