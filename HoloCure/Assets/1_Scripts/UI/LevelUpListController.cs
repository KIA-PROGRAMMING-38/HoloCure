using System;
using UnityEngine;

public class LevelUpListController : UIBase
{
    public event Action OnSelect;
    public event Action<int> OnSelectWeapon;

    [SerializeField] private LevelUpList[] _lists;
    private WeaponData[] _weaponLists;
    
    private void Start()
    {
        for (int i = 0; i < _lists.Length; ++i)
        {
            for (int j = 0; j < _lists.Length; ++j)
            {
                if (i == j)
                {
                    continue;
                }

                _lists[i].OnHoverForOtherList -= _lists[j].ActivateDefaultFrame;
                _lists[i].OnHoverForOtherList += _lists[j].ActivateDefaultFrame;
            }

            _lists[i].OnClickForController -= TriggerEventByClick;
            _lists[i].OnClickForController += TriggerEventByClick;
        }

        OnSelect -= PresenterManager.TriggerUIPresenter.DeActivateLevelUpUI;
        OnSelect += PresenterManager.TriggerUIPresenter.DeActivateLevelUpUI;
        OnSelectWeapon -= PresenterManager.TriggerUIPresenter.SendSelectedID;
        OnSelectWeapon += PresenterManager.TriggerUIPresenter.SendSelectedID;

        PresenterManager.TriggerUIPresenter.OnGetWeaponDatas -= GetWeaponList;
        PresenterManager.TriggerUIPresenter.OnGetWeaponDatas += GetWeaponList;
    }
    private void TriggerEventByClick(LevelUpList list)
    {
        int index = 0;
        for (int i = 0; i < _lists.Length; ++i)
        {
            if (list != _lists[i])
            {
                continue;
            }

            index = i;
            break;
        }

        SelectWeapon(index);
    }
    private void GetWeaponList(WeaponData[] weaponLists)
    {
        _weaponLists = weaponLists;

        for (int i = 0; i < 4; ++i)
        {
            _lists[i].GetWeaponData(_weaponLists[i]);
        }
    }

    private void SelectWeapon(int index)
    {
        OnSelectWeapon?.Invoke(_weaponLists[index].ID);
        OnSelect?.Invoke();
    }    
}
