using StringLiterals;
using System;
using UniRx.Triggers;
using UniRx;
using UnityEngine.EventSystems;
using UnityEngine;

public class SelectStageSubItem : UISubItem
{
    #region Enums    

    enum Buttons
    {
        StageSelectButton,
        StageMoveLeftButton,
        StageMoveRightButton,
        PlayButton
    }

    enum Images
    {
        StageImage
    }

    enum Texts
    {
        StageNumText,
        StageNameText
    }

    enum Objects
    {
        PlayButton
    }

    #endregion

    public ReactiveProperty<int> Index { get; private set; } = new();

    private SelectPopup _selectPopup;

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));
        GetObject((int)Objects.PlayButton).SetActive(false);

        SetupStage(Index.Value);
        _selectPopup = transform.parent.GetComponentAssert<SelectPopup>();

        foreach (Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            GetButton((int)buttonIndex).BindViewEvent(OnClickButton, ViewEvent.Click, this);
        }

        this.UpdateAsObservable()
            .Subscribe(OnKeyPress);
    }

    public void InitIndex(int value)
    {
        Index.Value = value;
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
            ProcessButton(IsPlayButtonActive() ? Buttons.PlayButton : Buttons.StageSelectButton);
        }
        else if (Input.GetButtonDown(InputLiteral.HORIZONTAL))
        {
            bool isRightKey = Input.GetAxisRaw(InputLiteral.HORIZONTAL) == 1;
            SwitchHorizontalButton(isRightKey);
        }
        else if (Input.GetButtonDown(InputLiteral.CANCEL))
        {
            if (IsPlayButtonActive())
            {
                OnCancelPlay();
            }
            else
            {
                OnCancelSelectStage();
            }
        }
    }

    private void SwitchHorizontalButton(bool isRightKey)
    {
        if (isRightKey)
        {
            OnClickStageMoveRightButton();
        }
        else
        {
            OnClickStageMoveLeftButton();
        }
    }

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.StageSelectButton: OnClickStageSelectButton(); break;
            case Buttons.StageMoveLeftButton: OnClickStageMoveLeftButton(); break;
            case Buttons.StageMoveRightButton: OnClickStageMoveRightButton(); break;
            case Buttons.PlayButton: OnClickPlayButton(); break;
            default: throw new ArgumentOutOfRangeException(nameof(button));
        }
    }

    private bool IsPlayButtonActive()
    {
        return GetObject((int)Objects.PlayButton).activeSelf;
    }

    private void OnClickStageSelectButton()
    {
        GetObject((int)Objects.PlayButton).SetActive(true);
    }

    private void OnClickPlayButton()
    {
        CloseSubItem();

        _selectPopup.GameStart();
    }

    private void OnClickStageMoveLeftButton()
    {
        if (IsPlayButtonActive()) { return; }

        Index.Value = Index.Value == 0
            ? Managers.Data.SelectStage.Count - 1
            : Index.Value - 1;

        SetupStage(Index.Value);
    }

    private void OnClickStageMoveRightButton()
    {
        if (IsPlayButtonActive()) { return; }

        Index.Value = Index.Value == Managers.Data.SelectStage.Count - 1
            ? 0
            : Index.Value + 1;

        SetupStage(Index.Value);
    }

    private void OnCancelPlay()
    {
        GetObject((int)Objects.PlayButton).SetActive(false);
    }

    private void OnCancelSelectStage()
    {
        CloseSubItem();

        _selectPopup.SetupModeSelect();
    }

    private void SetupStage(int stageIndex)
    {
        SelectStageData data = Managers.Data.SelectStage[stageIndex];
        GetImage((int)Images.StageImage).sprite = Managers.Resource.LoadSprite(data.Sprite);
        GetText((int)Texts.StageNameText).text = data.Name;
        GetText((int)Texts.StageNumText).text = (stageIndex + 1).ToString();
    }
}