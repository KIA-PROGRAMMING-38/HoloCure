using StringLiterals;
using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameClearPopup : UIPopup
{
    #region Enums

    enum Buttons
    {
        MainMenuButton
    }

    enum Images
    {
        MainMenuButton
    }

    enum Texts
    {
        MainMenuText
    }

    enum Objects
    {
        MainMenuButton,
        BG,
        ClearText,
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
        BindObject(typeof(Objects));

        SetupView();

        CurrentButton = _currentButton;

        foreach (Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            Button button = GetButton((int)buttonIndex);
            button.BindViewEvent(OnEnterButton, ViewEvent.Enter, this);
            button.BindViewEvent(OnClickButton, ViewEvent.Click, this);
        }

        this.UpdateAsObservable()
            .Subscribe(OnPressKey);

        Time.timeScale = 0;
    }

    private void SetupView()
    {
        GetObject((int)Objects.MainMenuButton).SetActive(false);

        InitMove();
    }

    private void InitMove()
    {
        _canvasGroup = GetObject((int)Objects.BG).GetComponentAssert<CanvasGroup>();
        _rectTransform = GetObject((int)Objects.ClearText).GetComponentAssert<RectTransform>();
        _initPos = _rectTransform.anchoredPosition;
        _startPos = _initPos + Vector2.up * 500;

        StartCoroutine(MoveCo());
    }
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector2 _initPos;
    private Vector2 _startPos;
    private const int GAME_CLEAR_TIME = 7;
    private float _elapsedTime;
    private IEnumerator MoveCo()
    {
        while (_elapsedTime <= GAME_CLEAR_TIME)
        {
            _canvasGroup.alpha = Mathf.Lerp(0, 1f, _elapsedTime / GAME_CLEAR_TIME);
            _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _initPos, _elapsedTime / GAME_CLEAR_TIME);
            _elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        GetObject((int)Objects.MainMenuButton).SetActive(true);
    }

    #region Event Handlers

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
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_OptionButton_0");
        GetText((int)buttonIndex).color = s_normalColor;
    }

    private void SetButtonHighlighted(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("hud_OptionButton_1");
        GetText((int)buttonIndex).color = s_highlightedColor;
    }

    #endregion

    #region Button Actions

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.MainMenuButton: OnClickMainMenuButton(); break;
            default: throw new ArgumentOutOfRangeException(nameof(button));
        }
    }

    private void OnClickMainMenuButton()
    {
        ClosePopupUI();

        Managers.Game.GameEnd();
    }

    #endregion
}