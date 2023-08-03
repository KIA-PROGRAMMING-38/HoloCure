using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HudInventorySubItem : UISubItem
{
    #region Enums

    enum Images
    {
        WeaponIconImage_1,
        WeaponIconImage_2,
        WeaponIconImage_3,
        WeaponIconImage_4,
        WeaponIconImage_5,
        WeaponIconImage_6,
        WeaponLevelFrameImage_1,
        WeaponLevelFrameImage_2,
        WeaponLevelFrameImage_3,
        WeaponLevelFrameImage_4,
        WeaponLevelFrameImage_5,
        WeaponLevelFrameImage_6,
        WeaponLevelNumImage_1,
        WeaponLevelNumImage_2,
        WeaponLevelNumImage_3,
        WeaponLevelNumImage_4,
        WeaponLevelNumImage_5,
        WeaponLevelNumImage_6,
        EquipmentIconImage_1,
        EquipmentIconImage_2,
        EquipmentIconImage_3,
        EquipmentIconImage_4,
        EquipmentIconImage_5,
        EquipmentIconImage_6,
        EquipmentLevelFrameImage_1,
        EquipmentLevelFrameImage_2,
        EquipmentLevelFrameImage_3,
        EquipmentLevelFrameImage_4,
        EquipmentLevelFrameImage_5,
        EquipmentLevelFrameImage_6,
        EquipmentLevelNumImage_1,
        EquipmentLevelNumImage_2,
        EquipmentLevelNumImage_3,
        EquipmentLevelNumImage_4,
        EquipmentLevelNumImage_5,
        EquipmentLevelNumImage_6
    }

    #endregion

    public override void Init()
    {
        base.Init();

        BindImage(typeof(Images));
        InitImages();

        Managers.Game.VTuber.Inventory.WeaponCount.BindModelEvent(UpdateWeaponImages, this);
    }

    private void InitImages()
    {
        GetImage((int)Images.WeaponLevelFrameImage_1).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelFrameImage_2).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelFrameImage_3).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelFrameImage_4).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelFrameImage_5).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelFrameImage_6).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelNumImage_1).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelNumImage_2).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelNumImage_3).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelNumImage_4).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelNumImage_5).gameObject.SetActive(false);
        GetImage((int)Images.WeaponLevelNumImage_6).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelFrameImage_1).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelFrameImage_2).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelFrameImage_3).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelFrameImage_4).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelFrameImage_5).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelFrameImage_6).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelNumImage_1).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelNumImage_2).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelNumImage_3).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelNumImage_4).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelNumImage_5).gameObject.SetActive(false);
        GetImage((int)Images.EquipmentLevelNumImage_6).gameObject.SetActive(false);
    }

    #region UI Appearance Update

    private void UpdateWeaponImages(int weaponIndex)
    {
        if (weaponIndex <= 0) { return; }
        weaponIndex -= 1;

        Weapon weapon = Managers.Game.VTuber.Inventory.Weapons[weaponIndex];
        SetupWeaponIcon(weaponIndex, weapon.Id);

        weapon.Level.BindModelEvent(level => UpdateWeaponLevelImage(level, weaponIndex, weapon.Id), this);
    }

    private void UpdateWeaponLevelImage(int level, int weaponIndex, ItemID weaponId)
    {
        int baseIndex = (int)Images.WeaponLevelNumImage_1;
        int imageIndex = weaponIndex + baseIndex;

        WeaponType type = weaponId.ConvertToWeaponType();
        string path = GetNumSpritePath(type, level);

        GetImage(imageIndex).sprite = Managers.Resource.LoadSprite(path);
    }

    #endregion

    #region Helpers

    private readonly static Vector3 s_defaultScale = Vector3.one;
    private readonly static Color s_defaultColor = Color.white;
    private void SetupWeaponIcon(int weaponIndex, ItemID weaponId)
    {
        var (icon, frame, num) = GetWeaponImages(weaponIndex);
        WeaponType type = weaponId.ConvertToWeaponType();

        SetupImage(icon, Managers.Data.Item[weaponId].IconSprite);
        SetupImage(frame, GetFrameSpritePath(type));
        SetupImage(num, GetNumSpritePath(type, 1));

        static void SetupImage(Image image, string path)
        {
            image.gameObject.SetActive(true);
            image.gameObject.GetComponentAssert<RectTransform>().localScale = s_defaultScale;
            image.color = s_defaultColor;
            image.sprite = Managers.Resource.LoadSprite(path);
        }
    }

    private (Image icon, Image frame, Image num) GetWeaponImages(int weaponIndex)
    {
        Image icon = GetImage(weaponIndex, Images.WeaponIconImage_1);
        Image frame = GetImage(weaponIndex, Images.WeaponLevelFrameImage_1);
        Image num = GetImage(weaponIndex, Images.WeaponLevelNumImage_1);

        return (icon, frame, num);
    }

    private Image GetImage(int itemIndex, Images baseIndex)
    {
        return GetImage(itemIndex + (int)baseIndex);
    }

    private string GetFrameSpritePath(WeaponType type)
    {
        return ZString.Concat("ui_level_header_", GetWeaponColor(type), 0);
    }

    private string GetNumSpritePath(WeaponType type, int level)
    {
        return ZString.Concat("ui_digit_", GetWeaponColor(type), level);
    }

    private string GetWeaponColor(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Starting: return "pink_";
            case WeaponType.Common: return "white_";
            default: throw new ArgumentOutOfRangeException($"Invalid WeaponType: {type}");
        }
    }

    #endregion
}