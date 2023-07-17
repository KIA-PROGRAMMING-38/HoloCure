using System;
using UnityEngine;
using UnityEngine.UI;

public class GetBoxEndUI : UIBase
{
    public event Action<ItemID> OnSelectTake;

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
    private void ActivateGetBoxUI()
    {
        _canvas.enabled = true;
        SoundPool.GetPlayAudio(SoundID.BoxOpenEnd);
    }

    private void DeActivateGetBoxUI()
    {
        _canvas.enabled = false;
        _iconLights.SetActive(false);
        _particles.SetActive(false);
    }

    [SerializeField] private ItemList _list;
    private ItemID _id;
    private void GetWeaponList(ItemID[] ids)
    {
        Inventory inventory = Managers.Game.VTuber.Inventory;

        for (int i = 0; i < inventory.WeaponCount; ++i)
        {
            Weapon weapon = inventory.Weapons[i];
            if (weapon.Level >= 7)
            {
                continue;
            }

            for (int j = 0; j < ids.Length; ++j)
            {
                if (ids[j] == default)
                {
                    continue;
                }

                if (weapon.Id != ids[j])
                {
                    continue;
                }

                _id = weapon.Id;

                SetWeapon();

                return;
            }
        }
        while (true)
        {
            int randNum = UnityEngine.Random.Range(0, ids.Length);

            if (ids[randNum] == default)
            {
                continue;
            }

            _id = ids[randNum];

            SetWeapon();

            break;
        }
    }

    [SerializeField] private Image[] _icons;
    [SerializeField] private GameObject _iconLights;
    [SerializeField] private GameObject _particles;
    private void SetWeapon()
    {
        _list.GetItemData(Managers.Data.Item[_id]);
        _icons[1].sprite = _icons[0].sprite;
        _iconLights.SetActive(true);
        _particles.SetActive(true);
    }

    [SerializeField] private BoxButtonController _controller;
    private void SelectTake()
    {
        OnSelectTake?.Invoke(_id);
    }
}
