using Cysharp.Text;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

public class GetBoxEndSubItem : UISubItem
{
    #region Enums

    enum Buttons
    {
        TakeButton,
        DropButton
    }

    enum Images
    {
        TakeButton,
        DropButton,
        ItemTypeImage,
        IconImage,
        OnBoxIconImage
    }

    enum Texts
    {
        TakeText,
        DropText,
        ItemNameText,
        TypeText,
        DescriptionText
    }

    enum Objects
    {
        NewText,
        Cursor
    }

    #endregion

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

    private ItemID _item;
    private StatSubItem _statSubItem;
    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        _statSubItem = Managers.UI.OpenSubItem<StatSubItem>(transform);
        SetupItem();

        CurrentButton = _currentButton;

        foreach (Buttons buttonIndex in Enum.GetValues(typeof(Buttons)))
        {
            Button button = GetButton((int)buttonIndex);
            button.BindViewEvent(OnEnterButton, ViewEvent.Enter, this);
            button.BindViewEvent(OnClickButton, ViewEvent.Click, this);
        }

        this.UpdateAsObservable()
            .Subscribe(OnPressKey);

        Managers.Spawn.SpawnOpenBoxCoin();
        Managers.Spawn.SpawnOpenBoxParticle();
        Managers.Spawn.SpawnOpenedBoxParticle();

        Managers.Sound.Play(SoundID.BoxOpenEnd);
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

        Transform cursorTransform = GetObject((int)Objects.Cursor).transform;
        Transform buttonTransform = GetButton((int)buttonIndex).transform;

        cursorTransform.SetParent(buttonTransform, false);
        cursorTransform.localPosition = default;
    }

    private void SwitchNextButton(bool isUpKey)
    {
        int nextButtonIndex = isUpKey ? (int)CurrentButton - 1 : (int)CurrentButton + 1;
        Buttons nextButton = (Buttons)Mathf.Clamp(nextButtonIndex, (int)Buttons.TakeButton, (int)Buttons.DropButton);

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.ButtonMove);
    }

    private void SetupItem()
    {
        _item = Managers.Item.GetItemLists()[0];

        ItemType type = _item.ConvertToItemType();

        SetupCommonView(_item);

        switch (type)
        {
            case ItemType.Weapon: SetupWeaponView(_item); break;
            case ItemType.Equipment: break;
            case ItemType.Stat: SetupStatView(_item); break;
            default: throw new ArgumentOutOfRangeException(nameof(type));
        }
    }

    private void SetupCommonView(ItemID id)
    {
        ItemData data = Managers.Data.Item[id];

        GetImage((int)Images.IconImage).sprite = Managers.Resource.LoadSprite(data.IconSprite);
        GetImage((int)Images.OnBoxIconImage).sprite = Managers.Resource.LoadSprite(data.IconSprite);
        GetImage((int)Images.ItemTypeImage).sprite = Managers.Resource.LoadSprite(data.Type);
        GetText((int)Texts.ItemNameText).text = data.Name;
        GetObject((int)Objects.NewText).SetActive(false);
    }

    private void SetupWeaponView(ItemID id)
    {
        Inventory inventory = Managers.Game.VTuber.Inventory;
        Weapon weapon = null;
        for (int i = 0; i < inventory.WeaponCount.Value; ++i)
        {
            if (id != inventory.Weapons[i].Id) { continue; }
            weapon = inventory.Weapons[i];
        }

        int nextLevel = weapon == null
            ? 1
            : weapon.Level.Value + 1;

        WeaponLevelData levelData = Managers.Data.WeaponLevelTable[id][nextLevel];
        GetText((int)Texts.DescriptionText).text = levelData.Description;
        GetText((int)Texts.TypeText).text = ">> Weapon";
        if (nextLevel == 1)
        {
            GetObject((int)Objects.NewText).SetActive(true);
        }
        else
        {
            TMP_Text nameText = GetText((int)Texts.ItemNameText);
            nameText.text = ZString.Concat(nameText.text, " LV ", nextLevel);
        }
    }

    private void SetupStatView(ItemID id)
    {
        StatData data = Managers.Data.Stat[id];
        GetText((int)Texts.DescriptionText).text = data.Description;
        GetText((int)Texts.TypeText).text = ">> Stat";
    }

    #endregion

    #region Button Actions

    private void ProcessButton(Buttons button)
    {
        switch (button)
        {
            case Buttons.TakeButton: OnClickTakeButton(); break;
            case Buttons.DropButton: OnClickDropButton(); break;
            default: throw new ArgumentOutOfRangeException(nameof(button));
        }

        Managers.Sound.Play(SoundID.ButtonClick);
        Managers.Sound.UnPause(SoundType.BGM);
        Managers.Sound.BGMVolumeUp();
    }

    private void OnClickTakeButton()
    {
        Managers.Game.VTuber.Inventory.GetItem(_item);

        OnClickDropButton();
    }

    private void OnClickDropButton()
    {
        Managers.Spawn.StopSpawnOpenedBoxParticle();

        Time.timeScale = 1.0f;

        transform.parent.GetComponentAssert<GetBoxPopup>().ClosePopupUI();

        CloseSubItem();
    }

    #endregion

    private void OnDestroy()
    {
        _statSubItem.CloseSubItem();
    }
}