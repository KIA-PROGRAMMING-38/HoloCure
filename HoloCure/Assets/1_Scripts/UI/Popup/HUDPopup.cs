using Cysharp.Text;
using System.IO;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUDPopup : UIPopup
{
    enum Images
    {
        ExpGaugeImage,
        PortraitDisplayImage,
        HPGaugeImage,
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
    enum Texts
    {
        LevelText,
        CurHPText,
        MaxHPText,
        SecondText,
        MinuteText,
        CoinText,
        DefeatedEnemyText
    }
    public override void Init()
    {
        base.Init();

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Managers.Game.VTuber.Inventory.WeaponCount.BindModelEvent(UpdateWeaponImages, this);

        Managers.Game.VTuber.CurExp.BindModelEvent(UpdateExpGaugeImage, this);
        Managers.Game.VTuber.Id.BindModelEvent(UpdatePortraitDisplayImage, this);
        Managers.Game.VTuber.CurHealth.BindModelEvent(UpdateHPGaugeImage, this);

        Managers.Game.VTuber.Level.BindModelEvent(UpdateLevelText, this);
        Managers.Game.VTuber.CurHealth.BindModelEvent(UpdateCurHPText, this);
        Managers.Game.VTuber.MaxHealth.BindModelEvent(UpdateMaxHPText, this);
        Managers.Game.Seconds.BindModelEvent(UpdateSecondsText, this);
        Managers.Game.Minutes.BindModelEvent(UpdateMinutesText, this);
        Managers.Game.Coins.BindModelEvent(UpdateCoinText, this);
        Managers.Game.DefeatedEnemies.BindModelEvent(UpdateDefeatedEnemyText, this);
    }
    private void UpdateWeaponImages(int index)
    {
        if (index == 0) { return; }
        index -= 1;

        Weapon weapon = Managers.Game.VTuber.Inventory.Weapons[index];
        SetupWeaponIcon(index, weapon.Id);

        weapon.Level.BindModelEvent(level => UpdateWeaponLevelImage(level, index, weapon.Id), this);
    }
    private readonly static Vector3 s_defaultScale = Vector3.one;
    private void SetupWeaponIcon(int index, ItemID id)
    {
        ItemData data = Managers.Data.Item[id];

        int iconIndex = index + (int)Images.WeaponIconImage_1;
        int frameIndex = index + (int)Images.WeaponLevelFrameImage_1;
        int numIndex = index + (int)Images.WeaponLevelNumImage_1;

        Image icon = GetImage(iconIndex);
        Image frame = GetImage(frameIndex);
        Image num = GetImage(numIndex);

        frame.gameObject.SetActive(true);
        num.gameObject.SetActive(true);

        icon.gameObject.GetComponentAssert<RectTransform>().localScale = s_defaultScale;
        frame.gameObject.GetComponentAssert<RectTransform>().localScale = s_defaultScale;
        num.gameObject.GetComponentAssert<RectTransform>().localScale = s_defaultScale;

        WeaponType type = id.GetWeaponType();
        string framePath = ZString.Concat("ui_level_header_", GetWeaponColor(type), 0);
        string numPath = ZString.Concat("ui_digit_", GetWeaponColor(type), 1);

        icon.sprite = Managers.Resource.LoadSprite(data.IconSprite);
        frame.sprite = Managers.Resource.LoadSprite(framePath);
        num.sprite = Managers.Resource.LoadSprite(numPath);
    }
    private void UpdateWeaponLevelImage(int level, int index, ItemID id)
    {
        index += (int)Images.WeaponLevelNumImage_1;

        WeaponType type = id.GetWeaponType();
        string path = ZString.Concat("ui_digit_", GetWeaponColor(type), level);

        GetImage(index).sprite = Managers.Resource.LoadSprite(path);
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
    private void UpdateExpGaugeImage(int curExp)
    {
        float maxExp = Managers.Game.VTuber.MaxExp.Value;
        GetImage((int)Images.ExpGaugeImage).fillAmount = curExp / maxExp;
    }
    private void UpdatePortraitDisplayImage(VTuberID id)
    {
        VTuberData data = Managers.Data.VTuber[id];
        GetImage((int)Images.PortraitDisplayImage).sprite = Managers.Resource.LoadSprite(data.DisplaySprite);
    }
    private void UpdateHPGaugeImage(int CurHP)
    {
        float MaxHP = Managers.Game.VTuber.MaxHealth.Value;
        GetImage((int)Images.HPGaugeImage).fillAmount = CurHP / MaxHP;
    }
    private void UpdateLevelText(int level)
    {
        GetText((int)Texts.LevelText).text = level.ToString();
    }
    private void UpdateCurHPText(int CurHP)
    {
        GetText((int)Texts.CurHPText).text = CurHP.ToString();
    }
    private void UpdateMaxHPText(int MaxHP)
    {
        GetText((int)Texts.MaxHPText).text = MaxHP.ToString();
    }
    private void UpdateSecondsText(int seconds)
    {
        GetText((int)Texts.SecondText).text = seconds.ToString("D2");
    }
    private void UpdateMinutesText(int minutes)
    {
        GetText((int)Texts.MinuteText).text = minutes.ToString("D2");
    }
    private void UpdateCoinText(int coin)
    {
        GetText((int)Texts.CoinText).text = coin.ToString();
    }
    private void UpdateDefeatedEnemyText(int enemy)
    {
        GetText((int)Texts.DefeatedEnemyText).text = enemy.ToString();
    }
}