using System;
using UnityEngine;

namespace UI.Presenter
{
    public class InventoryPresenter
    {
        public event Action<int, Sprite> OnUpdateNewEquipment;
        public event Action<int, int> OnUpdateEquipmentLevel;
        public event Action OnResetInventory;

        public void UpdateNewEquipment(int ID, Sprite icon) => OnUpdateNewEquipment?.Invoke(ID, icon);
        public void UpdateEquipmentLevel(int ID, int level) => OnUpdateEquipmentLevel?.Invoke(ID, level);
        public void ResetInventory() => OnResetInventory?.Invoke();
    }
}