using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

public class SelectModeSubItem : UISubItem
{
    #region Enums

    enum Buttons
    {
        StageModeButton,
        EndlessModeButton
    }

    enum Objects
    {
        HighlightCursor
    }

    #endregion

    public ReactiveProperty<int> Index { get; private set; } = new();

    private Buttons _currentButton;
    private Buttons CurrentButton
    {
        get => _currentButton;
        set
        {
            SetButtonHighlighted(value);

            Index.Value = (int)value;
            _currentButton = value;
        }
    }

    private SelectPopup _selectPopup;

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindObject(typeof(Objects));

        CurrentButton = _currentButton;
        _selectPopup = transform.parent.GetComponentAssert<SelectPopup>();

        foreach (Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            Button button = GetButton((int)buttonIndex);
            button.BindViewEvent(OnEnterButton, ViewEvent.Enter, this);
            button.BindViewEvent(OnClickButton, ViewEvent.Click, this);
        }

        this.UpdateAsObservable()
            .Subscribe(OnKeyPress);
    }

    public void InitIndex(int value)
    {
        _currentButton = (Buttons)value;
    }

    private void OnEnterButton(PointerEventData eventData)
    {
        Buttons nextButton = Enum.Parse<Buttons>(eventData.pointerEnter.name);

        CurrentButton = nextButton;


    }

    private void OnClickButton(PointerEventData eventData)
    {
        Buttons button = Enum.Parse<Buttons>(eventData.pointerClick.name);

        ProcessButton(button);
    }

    private void OnKeyPress(Unit unit)
    {
        if (Input.GetButtonDown(Define.Input.CONFIRM))
        {
            ProcessButton(CurrentButton);
        }
        else if (Input.GetButtonDown(Define.Input.VERTICAL))
        {
            bool isUpKey = Input.GetAxisRaw(Define.Input.VERTICAL) == 1;
            SwitchVerticalButton(isUpKey);
        }
        else if (Input.GetButtonDown(Define.Input.CANCEL))
        {
            OnCancel();
        }
    }


    private void SetButtonHighlighted(Buttons buttonIndex)
    {
        Transform cursorTransform = GetObject((int)Objects.HighlightCursor).transform;
        Transform buttonTransform = GetButton((int)buttonIndex).transform;

        cursorTransform.SetParent(buttonTransform, false);
        cursorTransform.localPosition = default;
    }

    private void SwitchVerticalButton(bool isUpKey)
    {
        int nextButtonIndex = isUpKey ? (int)CurrentButton - 1 : (int)CurrentButton + 1;
        Buttons nextButton = (Buttons)Mathf.Clamp(nextButtonIndex, (int)Buttons.StageModeButton, (int)Buttons.EndlessModeButton);

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.SelectMove);
    }

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.StageModeButton: OnClickStageModeButton(); break;
            case Buttons.EndlessModeButton: break;
            default: throw new ArgumentOutOfRangeException(nameof(button));
        }

        Managers.Sound.Play(SoundID.SelectClick);
    }

    private void OnClickStageModeButton()
    {
        CloseSubItem();

        _selectPopup.SetupStageSelect();
    }

    private void OnCancel()
    {
        CloseSubItem();

        _selectPopup.SetupIdolSelect();

        Managers.Sound.Play(SoundID.ButtonBack);
    }
}
