using Cysharp.Text;
using UnityEngine.UI;

public class HUDInventoryView : View
{
    public Image[] WeaponIconImages { get; private set; }
    public Image[] EquipmentIconImages { get; private set; }
    public Image[] WeaponLevelFrameImages { get; private set; }
    public Image[] WeaponLevelNumImages { get; private set; }
    protected override void Init()
    {
        WeaponIconImages = new Image[6];
        EquipmentIconImages = new Image[6];
        WeaponLevelFrameImages = new Image[6];
        WeaponLevelNumImages = new Image[6];

        for (int i = 0; i < 6; ++i)
        {
            WeaponIconImages[i] = transform.FindAssert(ZString.Concat("Weapon", i + 1)).GetComponentAssert<Image>();
            WeaponLevelFrameImages[i] = WeaponIconImages[i].transform.FindAssert("WeaponLevelFrame Image").GetComponentAssert<Image>();
            WeaponLevelNumImages[i] = WeaponIconImages[i].transform.FindAssert("WeaponLevelNum Image").GetComponentAssert<Image>(); 

            EquipmentIconImages[i] = transform.FindAssert(ZString.Concat("Equipment", i + 1)).GetComponentAssert<Image>();
        }
    }
}