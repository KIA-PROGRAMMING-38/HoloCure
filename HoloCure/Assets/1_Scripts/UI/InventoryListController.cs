using UnityEngine;

public class InventoryListController : UIBase
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
    }
    private void GetNewEquipment(int ID, Sprite icon)
    {
        if (ID < 9000)
        {
            _weaponLists[_weaponCount].ID = ID;
            _weaponLists[_weaponCount].UpdateNewEquipment(icon);

            _weaponCount += 1;
        }
        else
        {
            // 아이템 리스트 처리 부분
        }
    }
    private void GetEquipmentLevel(int ID, int level)
    {
        if (ID < 9000)
        {
            for (int i = 0; i < _weaponCount; ++i)
            {
                if (_weaponLists[i].ID != ID)
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
}
