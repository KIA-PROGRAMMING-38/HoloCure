using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class LevelUpListController : UIBase
{
    public event Action OnSelect;
    public event Action<int> OnSelectItem;

    [SerializeField] private ItemList[] _lists;
    private int _hoveredListIndex;
    private ItemData[] _itemLists;
    
    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI -= StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI += StartGetKeyCoroutine;

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

        OnSelect -= PresenterManager.TriggerUIPresenter.DeActivateUI;
        OnSelect += PresenterManager.TriggerUIPresenter.DeActivateUI;
        OnSelectItem -= PresenterManager.TriggerUIPresenter.SendSelectedID;
        OnSelectItem += PresenterManager.TriggerUIPresenter.SendSelectedID;

        PresenterManager.TriggerUIPresenter.OnGetItemDatasForLevelUp -= GetItemList;
        PresenterManager.TriggerUIPresenter.OnGetItemDatasForLevelUp += GetItemList;
    }
    private void StartGetKeyCoroutine() => StartCoroutine(_getKeyCoroutine);
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
                bool upKey = Input.GetAxisRaw(InputLiteral.VERTICAL) == 1;

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
    private void GetHoveredList(ItemList list)
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
    private void TriggerEventByKey() => SelectItem(_hoveredListIndex);
    private void TriggerEventByClick(ItemList list)
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

        SelectItem(index);
    }
    private void GetItemList(ItemData[] itemLists)
    {
        _itemLists = itemLists;

        for (int i = 0; i < 4; ++i)
        {
            _lists[i].GetItemData(_itemLists[i]);
        }
    }

    private void SelectItem(int index)
    {
        StopCoroutine(_getKeyCoroutine);
        OnSelectItem?.Invoke(_itemLists[index].ID);
        OnSelect?.Invoke();
    }    
}
