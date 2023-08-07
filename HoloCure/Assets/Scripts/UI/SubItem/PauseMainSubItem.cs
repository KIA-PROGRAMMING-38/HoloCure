using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

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

    public void InitIndex(int index)
    {
        _currentButton = (Buttons)index;
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
            OnClickResumeButton();
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
        Buttons nextButton = (Buttons)Mathf.Clamp(nextButtonIndex, (int)Buttons.SkillsButton, (int)Buttons.QuitButton);

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.ButtonMove);
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

        Managers.Sound.Play(SoundID.ButtonClick);
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

        Managers.Sound.BGMVolumeUp();
    }

    private void OnClickQuitButton()
    {
        Managers.UI.OpenSubItem<PauseQuitSubItem>(transform.parent);

        CloseSubItem();
    }

    #endregion    
}