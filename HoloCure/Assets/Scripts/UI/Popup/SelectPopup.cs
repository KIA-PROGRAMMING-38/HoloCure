using StringLiterals;
using System.Collections;
using UnityEngine;

public class SelectPopup : UIPopup
{
    #region Enums

    enum Images
    {
        TitleImage,
        VTuberAnim,
        WeaponIconImage
    }

    enum Texts
    {
        VTuberNameText,
        MaxHpText,
        AttackText,
        SpeedText,
        CriticalText,
        WeaponNameText,
        WeaponDescriptionText
    }

    enum Objects
    {
        BG,
        StatFrame,
        GuideFrame,
        VTuberAnim,
    }

    #endregion

    public SelectIdolSubItem IdolSubItem { get; set; }
    public SelectModeSubItem ModeSubItem { get; set; }
    public SelectStageSubItem StageSubItem { get; set; }

    private VTuberID _selectedIdolID;
    private int _selectedIdol;
    private int _selectedMode;
    private int _selectedStage;

    public override void Init()
    {
        base.Init();

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        InitMove();
        SetupIdolSelect();

        Managers.Spawn.SpawnTriangle(GetObject((int)Objects.BG));
    }

    public void SetupIdolSelect()
    {
        int index = _selectedIdol;
        IdolSubItem = Managers.UI.OpenSubItem<SelectIdolSubItem>(transform);
        IdolSubItem.Index.BindModelEvent(OnIdolIndexChange, IdolSubItem);
        IdolSubItem.InitIndex(index);
    }

    public void SetupModeSelect()
    {
        int index = _selectedMode;
        ModeSubItem = Managers.UI.OpenSubItem<SelectModeSubItem>(transform);
        ModeSubItem.Index.BindModelEvent(OnModeIndexChange, ModeSubItem);
        ModeSubItem.InitIndex(index);
    }

    public void SetupStageSelect()
    {
        int index = _selectedStage;
        StageSubItem = Managers.UI.OpenSubItem<SelectStageSubItem>(transform);
        StageSubItem.Index.BindModelEvent(OnStageIndexChange, StageSubItem);
        StageSubItem.InitIndex(index);
    }

    public void ReturnToTitlePopup()
    {
        Managers.Spawn.StopSpawnTriangle();

        ClosePopupUI();

        Managers.UI.OpenPopup<TitlePopup>().InitButton(0);
    }

    public void GameStart()
    {
        Managers.Spawn.StopSpawnTriangle();

        ClosePopupUI();

        Managers.Game.InGameStart(_selectedIdolID, _selectedMode, _selectedStage + 1);
    }

    private void OnIdolIndexChange(int idolIndex)
    {
        SelectIdolData idolData = Managers.Data.SelectIdol[idolIndex];
        _selectedIdolID = idolData.IdolID;
        _selectedIdol = idolIndex;

        SetView(idolData);
        Move();
    }

    private void OnModeIndexChange(int modeIndex)
    {
        _selectedMode = modeIndex;
    }

    private void OnStageIndexChange(int stageIndex)
    {
        _selectedStage = stageIndex;
    }

    private void SetView(SelectIdolData data)
    {
        GetImage((int)Images.TitleImage).sprite = Managers.Resource.LoadSprite(data.TitleSprite);
        GetImage((int)Images.WeaponIconImage).sprite = Managers.Resource.LoadSprite(data.IconSprite);
        GetImage((int)Images.VTuberAnim).sprite = Managers.Resource.LoadSprite(data.DisplaySprite);

        GetText((int)Texts.VTuberNameText).text = data.VtuberName;
        GetText((int)Texts.MaxHpText).text = data.MaxHp;
        GetText((int)Texts.AttackText).text = data.Attack;
        GetText((int)Texts.SpeedText).text = data.Speed;
        GetText((int)Texts.CriticalText).text = data.Critical;
        GetText((int)Texts.WeaponNameText).text = data.WeaponName;
        GetText((int)Texts.WeaponDescriptionText).text = data.WeaponDescription;

        SetAnimation(data);
    }

    private void InitMove()
    {
        _statRectTransform = GetObject((int)Objects.StatFrame).GetComponentAssert<RectTransform>();
        _guideRectTransform = GetObject((int)Objects.GuideFrame).GetComponentAssert<RectTransform>();

        _statInitPos = _statRectTransform.anchoredPosition;
        _guideInitPos = _guideRectTransform.anchoredPosition;

        _statStartPos = _statInitPos + Vector2.left * 500;
        _guideStartPos = _guideInitPos + Vector2.right * 500;

        _moveCo = MoveCo();
    }
    private void Move()
    {
        _elapsedTime = 0;
        StartCoroutine(_moveCo);
    }
    private RectTransform _statRectTransform;
    private RectTransform _guideRectTransform;
    private Vector2 _statInitPos;
    private Vector2 _statStartPos;
    private Vector2 _guideInitPos;
    private Vector2 _guideStartPos;
    private float _elapsedTime;
    private IEnumerator _moveCo;
    private IEnumerator MoveCo()
    {
        while (true)
        {
            _statRectTransform.anchoredPosition = Vector2.Lerp(_statStartPos, _statInitPos, _elapsedTime / 0.1f);
            _guideRectTransform.anchoredPosition = Vector2.Lerp(_guideStartPos, _guideInitPos, _elapsedTime / 0.1f);

            if (_elapsedTime > 0.1f)
            {
                StopCoroutine(_moveCo);
            }

            _elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
    }

    private void SetAnimation(SelectIdolData data)
    {
        Animator animator = GetObject((int)Objects.VTuberAnim).GetComponentAssert<Animator>();
        var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = data.IdolID == VTuberID.None
            ? SetDefaultAnim(overrideController)
            : SetIdolAnim(overrideController, data);


        static AnimatorOverrideController SetDefaultAnim(AnimatorOverrideController overrideController)
        {
            overrideController[AnimClipLiteral.IDLE_EMPTY] = Managers.Resource.LoadAnimClip(AnimClipLiteral.IDLE_EMPTY);
            overrideController[AnimClipLiteral.RUN_EMPTY] = Managers.Resource.LoadAnimClip(AnimClipLiteral.RUN_EMPTY);
            return overrideController;
        }
        static AnimatorOverrideController SetIdolAnim(AnimatorOverrideController overrideController, SelectIdolData data)
        {
            overrideController[AnimClipLiteral.IDLE_EMPTY] = Managers.Resource.LoadAnimClip(data.VtuberName, AnimClipLiteral.IDLE_UI);
            overrideController[AnimClipLiteral.RUN_EMPTY] = Managers.Resource.LoadAnimClip(data.VtuberName, AnimClipLiteral.RUN_UI);
            return overrideController;
        }
    }
}