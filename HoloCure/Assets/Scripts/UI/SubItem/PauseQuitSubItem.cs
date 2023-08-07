using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

public class PauseQuitSubItem : UISubItem
{
    #region Enums

    enum Buttons
    {
        YesButton,
        NoButton
    }

    enum Images
    {
        YesButton,
        NoButton
    }

    enum Texts
    {
        YesText,
        NoText
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

    private static readonly Color NORMAL_COLOR = Color.white;
    private static readonly Color HIGHLIGHTED_COLOR = Color.black;

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
    }

    #region Event Handlers

    private void OnEnterButton(PointerEventData eventData)
    {
        var nextButtonTransform = eventData.pointerEnter.transform.parent;
        Buttons nextButton = Enum.Parse<Buttons>(nextButtonTransform.name);

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
        if (Input.GetButtonDown(Define.Input.CONFIRM))
        {
            ProcessButton(CurrentButton);
        }
        else if (Input.GetButtonDown(Define.Input.VERTICAL))
        {
            bool isUpKey = Input.GetAxisRaw(Define.Input.VERTICAL) == 1;
            SwitchNextButton(isUpKey);
        }
        else if (Input.GetButtonDown(Define.Input.CANCEL))
        {
            OnClickNoButton();
            Managers.Sound.Play(SoundID.ButtonBack);
        }
    }

    #endregion

    #region UI Appearance Update

    private void SetButtonNormal(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_Button_0");
        GetText((int)buttonIndex).color = NORMAL_COLOR;
    }

    private void SetButtonHighlighted(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_Button_1");
        GetText((int)buttonIndex).color = HIGHLIGHTED_COLOR;
    }

    private void SwitchNextButton(bool isUpKey)
    {
        int nextButtonIndex = isUpKey ? (int)CurrentButton - 1 : (int)CurrentButton + 1;
        Buttons nextButton = (Buttons)Mathf.Clamp(nextButtonIndex, (int)Buttons.YesButton, (int)Buttons.NoButton);

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.ButtonMove);
    }

    #endregion

    #region Button Actions

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.YesButton: OnClickYesButton(); break;
            case Buttons.NoButton: OnClickNoButton(); break;
            default: throw new ArgumentOutOfRangeException(nameof(button));
        }

        Managers.Sound.Play(SoundID.ButtonClick);
    }

    private void OnClickYesButton()
    {
        CloseSubItem();

        transform.parent.GetComponentAssert<PausePopup>().ClosePopupUI();

        Managers.Game.OutgameStart();
    }

    private void OnClickNoButton()
    {
        Managers.UI.OpenSubItem<PauseMainSubItem>(transform.parent).InitIndex(5);

        CloseSubItem();
    }

    #endregion
}