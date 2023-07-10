using System;
using UnityEngine;

namespace UI.Presenter
{
    public class InventoryPresenter
    {
        public event Action<ItemID, Sprite> OnUpdateNewEquipment;
        public event Action<ItemID, int> OnUpdateEquipmentLevel;
        public event Action OnResetInventory;

        public void UpdateNewEquipment(ItemID id, Sprite icon) => OnUpdateNewEquipment?.Invoke(id, icon);
        public void UpdateEquipmentLevel(ItemID id, int level) => OnUpdateEquipmentLevel?.Invoke(id, level);
        public void ResetInventory() => OnResetInventory?.Invoke();
    }
}