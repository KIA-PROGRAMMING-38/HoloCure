using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class SelectIconController : UIBase
{
    public event Action<int> OnHoveredIcon;

    public event Action OnSelectVTuber;
    public event Action<VTuberID> OnSelectVTuberToUI;

    public event Action OnCancel;

    [SerializeField] private Transform _cursor;
    [SerializeField] private MyFlashButton[] _icons;
    private int _hoveredIconIndex;
    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        for (int i = 0; i < _icons.Length; ++i)
        {
            _icons[i].OnHoverForController -= GetHoveredIconIndex;
            _icons[i].OnHoverForController += GetHoveredIconIndex;

            _icons[i].OnClickForController -= TriggerEventByClick;
            _icons[i].OnClickForController += TriggerEventByClick;
        }

        OnCancel -= PresenterManager.TitleUIPresenter.DeActivateSelectUI;
        OnCancel += PresenterManager.TitleUIPresenter.DeActivateSelectUI;
    }
    public void StartGetKeyCoroutine()
    {
        _delayTime = 0;
        StartCoroutine(_getKeyCoroutine);
    }
    private void StopGetKeyCoroutine() => StopCoroutine(_getKeyCoroutine);
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

                if (upKey && _hoveredIconIndex > 4)
                {
                    _hoveredIconIndex -= 5;
                    _cursor.SetParent(_icons[_hoveredIconIndex].transform, false);
                    HoveredIcon();
                }
                else if (false == upKey && _hoveredIconIndex < 25)
                {
                    _hoveredIconIndex += 5;
                    _cursor.SetParent(_icons[_hoveredIconIndex].transform, false);
                    HoveredIcon();
                }
            }
            else if (Input.GetButtonDown(InputLiteral.HORIZONTAL))
            {
                bool rightKey = Input.GetAxisRaw(InputLiteral.HORIZONTAL) == 1;

                if (rightKey && _hoveredIconIndex % 5 != 4)
                {
                    _hoveredIconIndex += 1;
                    _cursor.SetParent(_icons[_hoveredIconIndex].transform, false);
                    HoveredIcon();
                }
                else if (false == rightKey && _hoveredIconIndex % 5 != 0)
                {
                    _hoveredIconIndex -= 1;
                    _cursor.SetParent(_icons[_hoveredIconIndex].transform, false);
                    HoveredIcon();
                }
            }
            else if (Input.GetButtonDown(InputLiteral.CANCEL))
            {
                Cancel();
                yield return null;
            }

            yield return null;
        }
    }
    private void GetHoveredIconIndex(MyFlashButton icon)
    {
        _cursor.SetParent(icon.transform, false);
        for (int i = 0; i < _icons.Length; ++i)
        {
            if (icon != _icons[i])
            {
                continue;
            }

            _hoveredIconIndex = i;
            break;
        }
        HoveredIcon();
    }
    private void HoveredIcon() => OnHoveredIcon?.Invoke(_hoveredIconIndex);
    private void TriggerEventByKey() => IconSelect(_hoveredIconIndex);
    private void TriggerEventByClick(MyFlashButton icon)
    {
        IconSelect(_hoveredIconIndex);
    }
    private void IconSelect(int ID)
    {
        // 일단 숫자 처리하나 추후 데이터 테이블과 캐릭터 선택 아이디를 공유하게 변경해야함
        switch (ID)
        {
            case 2: // 이나
                StopGetKeyCoroutine();
                OnSelectVTuber?.Invoke();
                OnSelectVTuberToUI?.Invoke(VTuberID.Ninomae_Inanis);
                break;
        }
    }
    private void Cancel()
    {
        StopGetKeyCoroutine();
        OnCancel?.Invoke();
    }
}
