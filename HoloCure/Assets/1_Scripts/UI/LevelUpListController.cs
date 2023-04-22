using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class LevelUpListController : UIBase
{
    public event Action OnSelect;
    public event Action<int> OnSelectWeapon;

    [SerializeField] private LevelUpList[] _lists;
    private int _hoveredListIndex;
    private WeaponData[] _weaponLists;
    
    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI -= StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI += StartGetKeyCoroutine;

        PresenterManager.TriggerUIPresenter.OnResume -= StopGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnResume += StopGetKeyCoroutine;

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

            _lists[i].OnHoverForController -= GetHoveredList;
            _lists[i].OnHoverForController += GetHoveredList;

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
    private void StartGetKeyCoroutine() => StartCoroutine(_getKeyCoroutine);
    private void StopGetKeyCoroutine() => StopCoroutine(_getKeyCoroutine);
    private IEnumerator _getKeyCoroutine;
    private IEnumerator GetKeyCoroutine()
    {
        while (true)
        {
            if (Input.GetButtonDown(InputLiteral.CONFIRM))
            {
                TriggerEventByKey();
                yield return null;
            }
            else if (Input.GetButtonDown(InputLiteral.VERTICAL))
            {
                bool upKey = Input.GetAxisRaw(InputLiteral.VERTICAL) == 1 ? true : false;

                if (upKey && _hoveredListIndex != 0)
                {
                    _hoveredListIndex -= 1;
                    _lists[_hoveredListIndex].HoveredByKey();
                }
                else if (false == upKey && _hoveredListIndex != _lists.Length - 1)
                {
                    _hoveredListIndex += 1;
                    _lists[_hoveredListIndex].HoveredByKey();
                }
            }

            yield return null;
        }
    }
    private void GetHoveredList(LevelUpList list)
    {
        for (int i = 0; i < _lists.Length; ++i)
        {
            if (list != _lists[i])
            {
                continue;
            }

            _hoveredListIndex = i;
            break;
        }
    }
    private void TriggerEventByKey() => SelectWeapon(_hoveredListIndex);
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
