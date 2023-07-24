using StringLiterals;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectIdolSubItem : UIBase
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
    private readonly static Vector3 CURSOR_LOCAL_POSITION = new Vector3(2, -3, 0);

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindObject(typeof(Objects));

        CurrentButton = (Buttons)Index.Value;
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
        Index.Value = value;
    }

    private void OnEnterButton(PointerEventData eventData)
    {
        var nextButtonTransform = eventData.pointerEnter.transform.parent;
        Buttons nextButton = Enum.Parse<Buttons>(nextButtonTransform.name);

        CurrentButton = nextButton;
    }

    private void OnClickButton(PointerEventData eventData)
    {
        ProcessButton();
    }

    private void OnKeyPress(Unit unit)
    {
        if (Input.GetButtonDown(InputLiteral.CONFIRM))
        {
            ProcessButton();
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
        cursorTransform.localPosition = CURSOR_LOCAL_POSITION;
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
    }

    private void ProcessButton()
    {
        Managers.Resource.Destroy(gameObject);

        _selectPopup.SetupModeSelect();
    }

    private void OnCancel()
    {
        Managers.Resource.Destroy(gameObject);

        _selectPopup.ReturnToTitlePopup();
    }
}