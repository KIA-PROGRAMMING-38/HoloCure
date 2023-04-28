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

        PresenterManager.TriggerUIPresenter.OnGetWeaponDatasForBox -= GetWeaponList;
        PresenterManager.TriggerUIPresenter.OnGetWeaponDatasForBox += GetWeaponList;

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
    private WeaponData _data;
    private void GetWeaponList(WeaponData[] weaponLists)
    {
        for (int i = 0; i < Inventory.WeaponCount; ++i)
        {
            WeaponData weapon = Inventory.Weapons[i].WeaponData;
            if (weapon.CurrentLevel >= 7)
            {
                continue;
            }

            for (int j = 0; j < weaponLists.Length; ++j)
            {
                if (weaponLists[j] == null) // �ι��� �ӽ��ڵ�
                {
                    continue;
                }

                if (weapon != weaponLists[j])
                {
                    continue;
                }

                _data = weapon;

                SetWeapon();

                return;
            }
        }
        while (true) // �ι��� �ӽ��ڵ�
        {
            int randNum = UnityEngine.Random.Range(0, weaponLists.Length);

            if (weaponLists[randNum] == null) // �ι��� �ӽ��ڵ�
            {
                continue;
            }

            _data = weaponLists[randNum];

            SetWeapon();

            break; // �ι��� �ӽ��ڵ�
        }
    }

    [SerializeField] private Image[] _icons;
    [SerializeField] private GameObject _iconLights;
    [SerializeField] private GameObject _particles;
    private void SetWeapon()
    {
        _list.GetWeaponData(_data);
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
