using StringLiterals;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpList : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public event Action OnHoverForOtherList;
    public event Action<LevelUpList> OnClickForController;
    [Header("HoverCursor")]
    [SerializeField] private GameObject _defaultFrame;
    [SerializeField] private GameObject _hoveredFrame;
    [SerializeField] private GameObject _selectCursor;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _levelNumText;
    [SerializeField] private TextMeshProUGUI _newText;
    [SerializeField] private TextMeshProUGUI _typeText; // 추후 추가 구현
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [Header("Image")]
    [SerializeField] private Image _itemTypeImage;
    [SerializeField] private Image _iconFrameImage; // 추후 추가 구현
    [SerializeField] private Image _iconImage;
    [Header("Sprites")]
    [SerializeField] private Sprite[] _itemTypeSprites;
    [SerializeField] private Sprite[] _iconFrameSprites;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverForOtherList?.Invoke();
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
    public void GetWeaponData(WeaponData weaponData)
    {
        _nameText.text = weaponData.DisplayName;

        if (weaponData.CurrentLevel == 0)
        {
            _levelText.enabled = false;
            _levelNumText.enabled = false;
            _newText.enabled = true;
        }
        else
        {
            _levelText.enabled = true;
            _levelNumText.enabled = true;
            _newText.enabled = false;

            if (weaponData.CurrentLevel == 6)
            {
                _levelNumText.text = weaponData.ID < 8000 ? ItemLiteral.MAX : ItemLiteral.AWAKENED;
            }
            else
            {
                _levelNumText.text = NumLiteral.GetNumString(weaponData.CurrentLevel + 1);
            }
        }

        _descriptionText.text = weaponData.Description[weaponData.CurrentLevel + 1];

        switch (weaponData.Type)
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

        _iconImage.sprite = weaponData.Icon;
    }
}
