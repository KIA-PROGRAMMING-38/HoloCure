using StringLiterals;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitlePopup : UIPopup
{
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
    private int _curIndex;
    private int _buttonCount;
    private static readonly Color s_onNormalColor = Color.white;
    private static readonly Color s_onHighlightedColor = Color.black;
    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        _buttonCount = Enum.GetNames(typeof(Buttons)).Length;

        for (int i = 0; i < _buttonCount; ++i)
        {
            Button button = GetButton(i);
            button.BindViewEvent(OnEnterButton, ViewEvent.Enter, this);
            button.BindViewEvent(OnClickButton, ViewEvent.Click, this);
        }
        this.UpdateAsObservable()
            .Subscribe(CheckKeyPress);

        Managers.Spawn.SpawnTriangle();
    }
    private void OnEnterButton(PointerEventData eventData)
    {
        int index = (int)Enum.Parse(typeof(Buttons), eventData.pointerEnter.transform.parent.name);

        SetNormal(_curIndex);
        SetHighlighted(index);

        _curIndex = index;
    }
    private void SetNormal(int index)
    {
        GetImage(index).sprite = Managers.Resource.LoadSprite("hud_OptionButton_0");
        GetText(index).color = s_onNormalColor;
    }
    private void SetHighlighted(int index)
    {
        GetImage(index).sprite = Managers.Resource.LoadSprite("hud_OptionButton_1");
        GetText(index).color = s_onHighlightedColor;
    }
    private void OnClickButton(PointerEventData eventData)
    {
        int index;
        if (eventData == null)
        {
            index = _curIndex;
        }
        else
        {
            index = (int)Enum.Parse(typeof(Buttons), eventData.pointerClick.name);
        }

        switch ((Buttons)index)
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
    private void CheckKeyPress(Unit unit)
    {
        if (Input.GetButtonDown(InputLiteral.CONFIRM))
        {
            OnClickButton(null);
        }
        else if (Input.GetButtonDown(InputLiteral.VERTICAL))
        {
            bool isUpKey = Input.GetAxisRaw(InputLiteral.VERTICAL) == 1;
            KeyMove(isUpKey);
        }
    }
    private void KeyMove(bool isUpKey)
    {
        SetNormal(_curIndex);

        if (isUpKey)
        {
            _curIndex = Mathf.Max(0, _curIndex - 1);
        }
        else
        {
            _curIndex = Mathf.Min(_buttonCount - 1, _curIndex + 1);
        }

        SetHighlighted(_curIndex);
    }
}