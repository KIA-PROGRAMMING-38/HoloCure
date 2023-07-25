using Cysharp.Text;
using StringLiterals;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpPopup : UIPopup
{
    #region Enums

    enum Buttons
    {
        ItemButton_1,
        ItemButton_2,
        ItemButton_3,
        ItemButton_4
    }

    enum Images
    {
        ItemButton_1,
        ItemButton_2,
        ItemButton_3,
        ItemButton_4,
        ItemTypeImage_1,
        ItemTypeImage_2,
        ItemTypeImage_3,
        ItemTypeImage_4,
        IconImage_1,
        IconImage_2,
        IconImage_3,
        IconImage_4,
    }

    enum Texts
    {
        ItemNameText_1,
        ItemNameText_2,
        ItemNameText_3,
        ItemNameText_4,
        TypeText_1,
        TypeText_2,
        TypeText_3,
        TypeText_4,
        DescriptionText_1,
        DescriptionText_2,
        DescriptionText_3,
        DescriptionText_4
    }

    enum Objects
    {
        NewText_1,
        NewText_2,
        NewText_3,
        NewText_4,
        HighlightCursor
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

    private ItemID[] _items;
    private StatSubItem _statSubItem;

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        _statSubItem = Managers.UI.OpenSubItem<StatSubItem>(transform);
        SetupItemButtons();

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

        Managers.Sound.BGMVolumeDown();
        Managers.Sound.Play(SoundID.LevelUp);
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
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("ui_menu_upgrade_window_0");
    }

    private void SetButtonHighlighted(Buttons buttonIndex)
    {
        GetImage((int)buttonIndex).sprite = Managers.Resource.LoadSprite("ui_menu_upgrade_window_selected_0");

        Transform cursorTransform = GetObject((int)Objects.HighlightCursor).transform;
        Transform buttonTransform = GetButton((int)buttonIndex).transform;

        cursorTransform.SetParent(buttonTransform, false);
        cursorTransform.localPosition = default;
    }

    private void SwitchNextButton(bool isUpKey)
    {
        int nextButtonIndex = isUpKey ? (int)CurrentButton - 1 : (int)CurrentButton + 1;
        Buttons nextButton = (Buttons)Mathf.Clamp(nextButtonIndex, (int)Buttons.ItemButton_1, (int)Buttons.ItemButton_4);

        CurrentButton = nextButton;

        Managers.Sound.Play(SoundID.ButtonMove);
    }

    private void SetupItemButtons()
    {
        _items = Managers.Item.GetItemLists();

        for (int i = 0; i < _items.Length; ++i)
        {
            SetupCommonView(_items[i], i);

            ItemType type = _items[i].GetItemType();

            switch (type)
            {
                case ItemType.Weapon: SetupWeaponView(_items[i], i); break;
                case ItemType.Equipment: break;
                case ItemType.Stat: SetupStatView(_items[i], i); break;
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }

    private void SetupCommonView(ItemID id, int index)
    {
        ItemData data = Managers.Data.Item[id];
        int icon = index + (int)Images.IconImage_1;
        int itemType = index + (int)Images.ItemTypeImage_1;
        int name = index + (int)Texts.ItemNameText_1;
        int newText = index + (int)Objects.NewText_1;

        GetImage(icon).sprite = Managers.Resource.LoadSprite(data.IconSprite);
        GetImage(itemType).sprite = Managers.Resource.LoadSprite(data.Type);
        GetText(name).text = data.Name;
        GetObject(newText).SetActive(false);
    }

    private void SetupWeaponView(ItemID id, int index)
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
        int description = index + (int)Texts.DescriptionText_1;
        int typeText = index + (int)Texts.TypeText_1;
        GetText(description).text = levelData.Description;
        GetText(typeText).text = ">> Weapon";
        if (nextLevel == 1)
        {
            int newText = index + (int)Objects.NewText_1;
            GetObject(newText).SetActive(true);
        }
        else
        {
            int itemName = index + (int)Texts.ItemNameText_1;
            TMP_Text nameText = GetText(itemName);
            nameText.text = ZString.Concat(nameText.text, " LV ", nextLevel);
        }
    }

    private void SetupStatView(ItemID id, int index)
    {
        StatData data = Managers.Data.Stat[id];
        int description = index + (int)Texts.DescriptionText_1;
        int typeText = index + (int)Texts.TypeText_1;
        GetText(description).text = data.Description;
        GetText(typeText).text = ">> Stat";
    }

    #endregion

    #region Button Actions

    private void ProcessButton(Buttons button)
    {
        Managers.Game.VTuber.Inventory.GetItem(_items[(int)button]);

        ClosePopupUI();

        Time.timeScale = 1.0f;

        Managers.Sound.Play(SoundID.ButtonClick);
        Managers.Sound.BGMVolumeUp();
    }

    #endregion

    private void OnDestroy()
    {
        _statSubItem.CloseSubItem();
    }
}