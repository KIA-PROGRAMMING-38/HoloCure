using UnityEngine;

public class InventoryListController : UIBaseLegacy
{
    [SerializeField] private InventoryList[] _weaponLists;
    private int _weaponCount;

    [SerializeField] private InventoryList[] _itemLists;
    private int _itemCount;

    private void Start()
    {
        PresenterManager.InventoryPresenter.OnUpdateNewEquipment -= GetNewEquipment;
        PresenterManager.InventoryPresenter.OnUpdateNewEquipment += GetNewEquipment;

        PresenterManager.InventoryPresenter.OnUpdateEquipmentLevel -= GetEquipmentLevel;
        PresenterManager.InventoryPresenter.OnUpdateEquipmentLevel += GetEquipmentLevel;

        PresenterManager.InventoryPresenter.OnResetInventory -= ResetInventory;
        PresenterManager.InventoryPresenter.OnResetInventory += ResetInventory;
    }
    private void GetNewEquipment(ItemID id, Sprite icon)
    {
        if (id < ItemID.StatNone)
        {
            _weaponLists[_weaponCount].Id = id;
            _weaponLists[_weaponCount].UpdateNewEquipment(icon);

            _weaponCount += 1;
        }
        else
        {
            // 아이템 리스트 처리 부분
        }
    }
    private void GetEquipmentLevel(ItemID ID, int level)
    {
        if (ID < ItemID.StatNone)
        {
            for (int i = 0; i < _weaponCount; ++i)
            {
                if (_weaponLists[i].Id != ID)
                {
                    continue;
                }

                _weaponLists[i].UpdateEquipmentLevel(level);

                break;
            }
        }
        else
        {
            // 아이템 리스트 처리 부분
        }
    }
    private void ResetInventory()
    {
        for (int i = 0; i < _weaponLists.Length; ++i)
        {
            _weaponLists[i].ResetInventory();
        }
        _weaponCount = 0;
    }
}
