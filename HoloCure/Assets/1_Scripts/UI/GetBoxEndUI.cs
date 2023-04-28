using System;
using UnityEngine;
using UnityEngine.UI;

public class GetBoxEndUI : UIBase
{
    public event Action<int> OnSelectTake;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI -= ActivateGetBoxUI;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI += ActivateGetBoxUI;

        PresenterManager.TriggerUIPresenter.OnResume -= DeActivateGetBoxUI;
        PresenterManager.TriggerUIPresenter.OnResume += DeActivateGetBoxUI;

        PresenterManager.TriggerUIPresenter.OnGetItemDatasForBox -= GetWeaponList;
        PresenterManager.TriggerUIPresenter.OnGetItemDatasForBox += GetWeaponList;

        _controller.OnSelectTake -= SelectTake;
        _controller.OnSelectTake += SelectTake;

        OnSelectTake -= PresenterManager.TriggerUIPresenter.SendSelectedID;
        OnSelectTake += PresenterManager.TriggerUIPresenter.SendSelectedID;
    }
    private void ActivateGetBoxUI() => _canvas.enabled = true;
    private void DeActivateGetBoxUI()
    {
        _canvas.enabled = false;
        _iconLights.SetActive(false);
        _particles.SetActive(false);
    }

    [SerializeField] private ItemList _list;
    private ItemData _data;
    private void GetWeaponList(ItemData[] itemLists)
    {
        for (int i = 0; i < itemLists.Length; ++i)
        {
            switch ((ItemDataKindID)itemLists[i].DataKind)
            {
                case ItemDataKindID.Weapon:
                    break;
                case ItemDataKindID.Equipment:
                    break;
                case ItemDataKindID.Stat:
                    break;
            }
        }


        for (int i = 0; i < Inventory.WeaponCount; ++i)
        {
            WeaponData weapon = Inventory.Weapons[i].WeaponData;
            if (weapon.CurrentLevel >= 7)
            {
                continue;
            }

            for (int j = 0; j < itemLists.Length; ++j)
            {
                if (itemLists[j] == null) // 널방지 임시코드
                {
                    continue;
                }

                if (weapon != itemLists[j])
                {
                    continue;
                }

                _data = weapon;

                SetWeapon();

                return;
            }
        }
        while (true) // 널방지 임시코드
        {
            int randNum = UnityEngine.Random.Range(0, itemLists.Length);

            if (itemLists[randNum] == null) // 널방지 임시코드
            {
                continue;
            }

            _data = itemLists[randNum];

            SetWeapon();

            break; // 널방지 임시코드
        }
    }

    [SerializeField] private Image[] _icons;
    [SerializeField] private GameObject _iconLights;
    [SerializeField] private GameObject _particles;
    private void SetWeapon()
    {
        _list.GetItemData(_data);
        _icons[1].sprite = _icons[0].sprite;
        _iconLights.SetActive(true);
        _particles.SetActive(true);
    }

    [SerializeField] private BoxButtonController _controller;
    private void SelectTake()
    {
        OnSelectTake?.Invoke(_data.ID);
    }
}
