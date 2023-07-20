using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUDInventoryPopup : UIPopup
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

        Managers.Game.VTuber.Inventory.WeaponCount.BindModelEvent(UpdateWeaponImages, this);
    }

    private void UpdateWeaponImages(int index)
    {
        if (index == 0) { return; }
        index -= 1;

        Weapon weapon = Managers.Game.VTuber.Inventory.Weapons[index];
        SetupWeaponIcon(index, weapon.Id);

        weapon.Level.BindModelEvent(level => UpdateWeaponLevelImage(level, index, weapon.Id), this);
    }

    private void UpdateWeaponLevelImage(int level, int index, ItemID weaponId)
    {
        index += (int)Images.WeaponLevelNumImage_1;

        WeaponType type = weaponId.GetWeaponType();
        string path = GetNumPath(type, level);

        GetImage(index).sprite = Managers.Resource.LoadSprite(path);
    }

    private readonly static Vector3 s_defaultScale = Vector3.one;
    private readonly static Color s_defaultColor = Color.white;
    private void SetupWeaponIcon(int index, ItemID weaponId)
    {
        var (icon, frame, num) = GetImages(index);
        WeaponType type = weaponId.GetWeaponType();

        SetupImage(icon, Managers.Data.Item[weaponId].IconSprite);
        SetupImage(frame, GetFramePath(type));
        SetupImage(num, GetNumPath(type, 1));

        static void SetupImage(Image image, string path)
        {
            image.gameObject.SetActive(true);
            image.gameObject.GetComponentAssert<RectTransform>().localScale = s_defaultScale;
            image.color = s_defaultColor;
            image.sprite = Managers.Resource.LoadSprite(path);
        }
    }

    private (Image, Image, Image) GetImages(int index)
    {
        Image icon = GetImage(index, Images.WeaponIconImage_1);
        Image frame = GetImage(index, Images.WeaponLevelFrameImage_1);
        Image num = GetImage(index, Images.WeaponLevelNumImage_1);

        return (icon, frame, num);
    }

    private Image GetImage(int index, Images startIndex)
    {
        return GetImage(index + (int)startIndex);
    }

    private string GetFramePath(WeaponType type)
    {
        return ZString.Concat("ui_level_header_", GetWeaponColor(type), 0);
    }

    private string GetNumPath(WeaponType type, int level)
    {
        return ZString.Concat("ui_digit_", GetWeaponColor(type), level);
    }

    private string GetWeaponColor(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Starting: return "pink_";
            case WeaponType.Common: return "white_";
            default: Debug.Assert(false, $"Invalid WeaponType: {type}"); return null;
        }
    }
}