using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class LevelUpListController : UIBase
{
    public event Action OnSelect;
    public event Action<ItemID> OnSelectItem;

    [SerializeField] private ItemList[] _lists;
    private int _hoveredListIndex;
    private ItemID[] _ids;

    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI -= StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI += StartGetKeyCoroutine;

        for (int i = 0; i < _lists.Length; ++i)
        {
            for (int j = 0; j < _lists.Length; ++j)
            {
                if (i == j) { continue; }

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
    private void StartGetKeyCoroutine()
    {
        _delayTime = 0;
        StartCoroutine(_getKeyCoroutine);
    }
    private float _delayTime;
    private IEnumerator _getKeyCoroutine;
    private IEnumerator GetKeyCoroutine()
    {
        while (true)
        {
            while (_delayTime < 0.3f)
            {
                _delayTime += Time.unscaledDeltaTime;
                yield return null;
            }

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
                    SoundPool.GetPlayAudio(SoundID.ButtonMove);
                }
                else if (false == upKey && _hoveredListIndex != _lists.Length - 1)
                {
                    _hoveredListIndex += 1;
                    _lists[_hoveredListIndex].HoveredByKey();
                    SoundPool.GetPlayAudio(SoundID.ButtonMove);
                }
            }

            yield return null;
        }
    }
    private void GetHoveredList(ItemList list)
    {
        for (int i = 0; i < _lists.Length; ++i)
        {
            if (list != _lists[i]) { continue; }

            _hoveredListIndex = i;
            break;
        }
        SoundPool.GetPlayAudio(SoundID.ButtonMove);
    }
    private void TriggerEventByKey() => SelectItem(_hoveredListIndex);
    private void TriggerEventByClick(ItemList list)
    {
        int index = 0;
        for (int i = 0; i < _lists.Length; ++i)
        {
            if (list != _lists[i]) { continue; }

            index = i;
            break;
        }

        SelectItem(index);
    }
    private void GetItemList(ItemID[] ids)
    {
        _ids = ids;

        for (int i = 0; i < 4; ++i)
        {
            _lists[i].GetItemData(Managers.Data.Item[ids[i]]);
        }
    }

    private void SelectItem(int index)
    {
        StopCoroutine(_getKeyCoroutine);
        SoundPool.GetPlayAudio(SoundID.ButtonClick);
        OnSelectItem?.Invoke(_ids[index]);
        OnSelect?.Invoke();
    }
}
