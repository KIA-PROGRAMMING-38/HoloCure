using StringLiterals;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMainSubItem : UISubItem
{
    #region Enums

    enum Buttons
    {
        SkillsButton,
        StampsButton,
        CollabsButton,
        ResumeButton,
        SettingsButton,
        QuitButton
    }

    enum Images
    {
        SkillsButton,
        StampsButton,
        CollabsButton,
        ResumeButton,
        SettingsButton,
        QuitButton
    }

    enum Texts
    {
        SkillsText,
        StampsText,
        CollabsText,
        ResumeText,
        SettingsText,
        QuitText
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
    }

    public void InitIndex(int index)
    {
        _currentButton = (Buttons)index;
    }

    #region Event Handlers

    private void OnEnterButton(PointerEventData eventData)
    {
        var nextButtonTransform = eventData.pointerEnter.transform.parent;
        Buttons nextButton = Enum.Parse<Buttons>(nextButtonTransform.name);

        CurrentButton = nextButton;
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
        else if (Input.GetButtonDown(InputLiteral.VERTICAL))
        {
            bool isUpKey = Input.GetAxisRaw(InputLiteral.VERTICAL) == 1;
            SwitchNextButton(isUpKey);
        }
        else if (Input.GetButtonDown(InputLiteral.CANCEL))
        {
            OnClickResumeButton();
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

    private void SwitchNextButton(bool isUpKey)
    {
        int nextButtonIndex = isUpKey ? (int)CurrentButton - 1 : (int)CurrentButton + 1;
        Buttons nextButton = (Buttons)Mathf.Clamp(nextButtonIndex, (int)Buttons.SkillsButton, (int)Buttons.QuitButton);

        CurrentButton = nextButton;
    }

    #endregion

    #region Button Actions

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.SkillsButton: OnClickSkillsButton(); break;
            case Buttons.StampsButton: OnClickStampsButton(); break;
            case Buttons.CollabsButton: OnClickCollabsButton(); break;
            case Buttons.ResumeButton: OnClickResumeButton(); break;
            case Buttons.SettingsButton: break;
            case Buttons.QuitButton: OnClickQuitButton(); break;
            default: throw new ArgumentOutOfRangeException(nameof(button));
        }
    }

    private void OnClickSkillsButton()
    {
        Managers.UI.OpenSubItem<PauseSkillsSubItem>(transform.parent);

        CloseSubItem();
    }

    private void OnClickStampsButton()
    {
        Managers.UI.OpenSubItem<PauseStampsSubItem>(transform.parent);

        CloseSubItem();
    }

    private void OnClickCollabsButton()
    {
        Managers.UI.OpenSubItem<PauseCollabsSubItem>(transform.parent);

        CloseSubItem();
    }

    private void OnClickResumeButton()
    {
        CloseSubItem();

        transform.parent.GetComponentAssert<PausePopup>().ClosePopupUI();

        Time.timeScale = 1.0f;
    }

    private void OnClickQuitButton()
    {
        Managers.UI.OpenSubItem<PauseQuitSubItem>(transform.parent);

        CloseSubItem();
    }

    #endregion    
}