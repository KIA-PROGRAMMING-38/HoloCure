using StringLiterals;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitlePopup : UIPopup
{
    #region Enums

    enum Buttons
    {
        PlayButton,
        ShopButton,
        LeaderboardButton,
        AchievementsButton,
        SettingsButton,
        CreditsButton,
        QuitButton
    }

    enum Images
    {
        PlayButton,
        ShopButton,
        LeaderboardButton,
        AchievementsButton,
        SettingsButton,
        CreditsButton,
        QuitButton
    }

    enum Texts
    {
        PlayText,
        ShopText,
        LeaderboardText,
        AchievementsText,
        SettingsText,
        CreditsText,
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

        foreach (Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            Button button = GetButton((int)buttonIndex);
            button.BindViewEvent(OnEnterButton, ViewEvent.Enter, this);
            button.BindViewEvent(OnClickButton, ViewEvent.Click, this);
        }

        this.UpdateAsObservable()
            .Subscribe(OnPressKey);
            
        Managers.Spawn.SpawnTriangle();
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
    }

    #endregion

    #region UI Appearance Update

    private void SetButtonNormal(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_OptionButton_0");
        GetText((int)buttonIndex).color = s_normalColor;
    }

    private void SetButtonHighlighted(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_OptionButton_1");
        GetText((int)buttonIndex).color = s_highlightedColor;
    }

    private void SwitchNextButton(bool isUpKey)
    {
        int nextButtonIndex = (isUpKey) ? (int)CurrentButton - 1 : (int)CurrentButton + 1;
        Buttons nextButton = (Buttons)Mathf.Clamp(nextButtonIndex, (int)Buttons.PlayButton, (int)Buttons.QuitButton);

        CurrentButton = nextButton;
    }

    #endregion

    #region Button Actions

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.PlayButton: OnClickPlayButton(); break;
            case Buttons.QuitButton: OnClickQuitButton(); break;
            default: break;
        }
    }

    private void OnClickPlayButton()
    {
        // Managers.UI.OpenPopupUI<SelectPopup>(); SelectPopup이 구현되어야합니다.
        Managers.UI.ClosePopupUI(this);
        Managers.Spawn.StopSpawnTriangle();
    }

    private void OnClickQuitButton()
    {
        Application.Quit();
    }

    #endregion    
}