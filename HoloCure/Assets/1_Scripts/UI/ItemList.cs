using Cysharp.Text;
using StringLiterals;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemList : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public event Action OnHoverForOtherList;
    public event Action<ItemList> OnHoverForController;
    public event Action<ItemList> OnClickForController;
    [Header("HoverCursor")]
    [SerializeField] private GameObject _defaultFrame;
    [SerializeField] private GameObject _hoveredFrame;
    [SerializeField] private GameObject _selectCursor;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _levelNumText;
    [SerializeField] private TextMeshProUGUI _newText;
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [Header("Image")]
    [SerializeField] private Image _itemTypeImage;
    [SerializeField] private Image _iconFrameImage; // 추후 추가 구현
    [SerializeField] private Image _iconImage;
    [Header("Sprites")]
    [SerializeField] private Sprite[] _itemTypeSprites;
    [SerializeField] private Sprite[] _iconFrameSprites;
    public void HoveredByKey()
    {
        OnHoverForOtherList?.Invoke();
        _defaultFrame.SetActive(false);
        _hoveredFrame.SetActive(true);
        _selectCursor.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverForOtherList?.Invoke();
        OnHoverForController?.Invoke(this);
        _defaultFrame.SetActive(false);
        _hoveredFrame.SetActive(true);
        _selectCursor.SetActive(true);
    }
    public void ActivateDefaultFrame()
    {
        _defaultFrame.SetActive(true);
        _hoveredFrame.SetActive(false);
        _selectCursor.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickForController?.Invoke(this);
    }
    public void GetItemData(ItemData data)
    {
        SetCommonData(data);

        switch (data.Id)
        {
            case < ItemID.StatNone:
                SetWeaponData(data);
                break;
            default:
                GetStatData(data);
                break;
        }
    }
    private void SetCommonData(ItemData data)
    {
        _nameText.text = data.Name;

        _levelText.enabled = false;
        _levelNumText.enabled = false;
        _newText.enabled = false;
        _itemTypeImage.enabled = false;
    }
    private const string WEAPON = "Weapon";
    private void SetWeaponData(ItemData data)
    {
        _typeText.text = WEAPON;
        _iconFrameImage.sprite = _iconFrameSprites[0];
        _iconImage.sprite = Managers.Resource.Sprites.Load(ZString.Concat(PathLiteral.SPRITE, PathLiteral.WEAPON, data.IconSprite));

        Weapon weapon = default;
        for (int i = 0; i < Inventory.WeaponCount; ++i)
        {
            if (Inventory.Weapons[i].Id != data.Id)
            {
                continue;
            }

            weapon = Inventory.Weapons[i];
        }

        if (weapon.Level == 0)
        {
            _newText.enabled = true;
        }
        else
        {
            _levelText.enabled = true;
            _levelNumText.enabled = true;
            _newText.enabled = false;

            if (weapon.Level == 6)
            {
                _levelNumText.text = weapon.Id < ItemID.StartingNone ? ItemLiteral.MAX : ItemLiteral.AWAKENED;
            }
            else
            {
                _levelNumText.text = NumLiteral.GetNumString(weapon.Level + 1);
            }
        }

        _descriptionText.text = Managers.Data.WeaponLevelTable[data.Id][weapon.Level + 1].Description;

        _itemTypeImage.enabled = true;
        switch (data.Type)
        {
            case ItemLiteral.MELEE:
                _itemTypeImage.sprite = _itemTypeSprites[0];
                break;
            case ItemLiteral.RANGED:
                _itemTypeImage.sprite = _itemTypeSprites[1];
                break;
            case ItemLiteral.MULTISHOT:
                _itemTypeImage.sprite = _itemTypeSprites[2];
                break;
        }
    }
    private const string STAT_UP = "StatUp";
    private void GetStatData(ItemData data)
    {
        _typeText.text = STAT_UP;
        _iconFrameImage.sprite = _iconFrameSprites[2];
        _descriptionText.text = Managers.Data.Stat[data.Id].Description;
        _iconImage.sprite = Managers.Resource.Sprites.Load(ZString.Concat(PathLiteral.SPRITE, PathLiteral.UI, data.IconSprite));
    }
}
