using StringLiterals;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectIdolSubItem : UISubItem
{
    #region Enums

    enum Buttons
    {
        IdolButton_01,
        IdolButton_02,
        IdolButton_03,
        IdolButton_04,
        IdolButton_05,
        IdolButton_06,
        IdolButton_07,
        IdolButton_08,
        IdolButton_09,
        IdolButton_10,
        IdolButton_11,
        IdolButton_12,
        IdolButton_13,
        IdolButton_14,
        IdolButton_15,
        IdolButton_16,
        IdolButton_17,
        IdolButton_18,
        IdolButton_19,
        IdolButton_20,
        IdolButton_21,
        IdolButton_22,
        IdolButton_23,
        IdolButton_24,
        IdolButton_25,
        IdolButton_26,
        IdolButton_27,
        IdolButton_28,
        IdolButton_29,
        IdolButton_30
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

        Managers.Sound.Play(SoundID.SelectMove);
    }

    private void OnClickButton(PointerEventData eventData)
    {
        Buttons button = Enum.Parse<Buttons>(eventData.pointerClick.name);

        ProcessButton(button);
    }

    private void OnKeyPress(Unit unit)
    {
        if (Input.GetButtonDown(InputLiteral.CONFIRM))
        {
            ProcessButton(CurrentButton);
        }
        else if (Input.GetButtonDown(InputLiteral.VERTICAL))
        {
            bool isUpKey = Input.GetAxisRaw(InputLiteral.VERTICAL) == 1;
            SwitchVerticalButton(isUpKey);
        }
        else if (Input.GetButtonDown(InputLiteral.HORIZONTAL))
        {
            bool isRightKey = Input.GetAxisRaw(InputLiteral.HORIZONTAL) == 1;
            SwitchHorizontalButton(isRightKey);
        }
        else if (Input.GetButtonDown(InputLiteral.CANCEL))
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
        int currentRow = (int)CurrentButton / 5;
        int currentCol = (int)CurrentButton % 5;

        int nextRow = isUpKey ? currentRow - 1 : currentRow + 1;
        nextRow = Mathf.Clamp(nextRow, 0, 5);

        int nextButtonIndex = nextRow * 5 + currentCol;
        Buttons nextButton = (Buttons)nextButtonIndex;

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.SelectMove);
    }

    private void SwitchHorizontalButton(bool isRightKey)
    {
        int currentRow = (int)CurrentButton / 5;
        int currentCol = (int)CurrentButton % 5;

        int nextCol = isRightKey ? currentCol + 1 : currentCol - 1;
        nextCol = Mathf.Clamp(nextCol, 0, 4);

        int nextButtonIndex = currentRow * 5 + nextCol;
        Buttons nextButton = (Buttons)nextButtonIndex;

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.SelectMove);
    }

    private void ProcessButton(Buttons button)
    {
        SelectIdolData data = Managers.Data.SelectIdol[(int)button];
        if (data.IdolID == VTuberID.None) { return; }

        CloseSubItem();

        _selectPopup.SetupModeSelect();

        Managers.Sound.Play(SoundID.SelectClick);
    }

    private void OnCancel()
    {
        CloseSubItem();

        _selectPopup.ReturnToTitlePopup();

        Managers.Sound.Play(SoundID.ButtonBack);
    }
}