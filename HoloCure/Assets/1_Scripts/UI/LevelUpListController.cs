using System;
using UnityEngine;

public class LevelUpListController : UIBase
{
    public event Action OnClick;
    public event Action<WeaponID> OnSelectWeapon;

    [SerializeField] private LevelUpList[] _lists;
    private WeaponID[] _weaponLists;
    
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
    private void GetWeaponList(WeaponID[] weaponLists)
    {
        _weaponLists = weaponLists;
    }
    private void SelectWeapon(int index)
    {
        OnSelectWeapon?.Invoke(_weaponLists[index]);
    }    
}
