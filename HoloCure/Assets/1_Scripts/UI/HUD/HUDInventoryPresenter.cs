using Cysharp.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HUDInventoryPresenter : Presenter
{
    private HUDInventoryView _inventoryView;
    protected override void InitView()
    {
        _inventoryView = transform.FindAssert("HUD Inventory View").GetComponentAssert<HUDInventoryView>();
    }
    protected override void OnRelease()
    {
        _inventoryView = default;
    }
    protected override void OnUpdatedModel()
    {
        Inventory inventory = Managers.Game.VTuber.Inventory;

        inventory.WeaponCount.Subscribe(SubscribeNewWeapon).AddTo(CompositeDisposable);
    }
    private void SubscribeNewWeapon(int index)
    {
        if (index == 0) { return; }
        index -= 1;

        Weapon weapon = Managers.Game.VTuber.Inventory.Weapons[index];
        SetUpIcon(index, weapon.Id);

        weapon.Level.Subscribe(level => UpdateWeaponLevelImage(level, index)).AddTo(CompositeDisposable);
    }
    private void SetUpIcon(int index, ItemID id)
    {
        Image weaponIcon = _inventoryView.WeaponIconImages[index];
        weaponIcon.gameObject.GetComponentAssert<RectTransform>().localScale = Vector3.one;
        _inventoryView.WeaponLevelFrameImages[index].gameObject.SetActive(true);
        _inventoryView.WeaponLevelNumImages[index].gameObject.SetActive(true);
        weaponIcon.sprite = Managers.Resource.LoadSprite(Managers.Data.Item[id].IconSprite);
    }
    private void UpdateWeaponLevelImage(int level, int index)
    {
        Weapon weapon = Managers.Game.VTuber.Inventory.Weapons[index];
        ItemType type = weapon.Id.GetItemType();
        switch (type)
        {
            case ItemType.StartingWeapon:
                _inventoryView.WeaponLevelNumImages[index].sprite = Managers.Resource.LoadSprite(ZString.Concat("ui_digit_pink_", level));
                break;
            case ItemType.CommonWeapon:
                _inventoryView.WeaponLevelNumImages[index].sprite = Managers.Resource.LoadSprite(ZString.Concat("ui_digit_white_", level));
                break;
            default: Debug.Assert(false, $"Invalid Value: {weapon}, {type}, {level}"); return;
        }
    }
}