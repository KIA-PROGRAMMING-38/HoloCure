using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
public enum BoxButtonID
{
    Take = 0,
    Drop = 1
}

public class BoxButtonController : UIBase
{
    public event Action OnSelect;
    public event Action OnSelectTake;

    [SerializeField] private MyButton[] _buttons;
    private int _hoveredButtonIndex;

    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI -= StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI += StartGetKeyCoroutine;

        //for (int i = 0; i < _buttons.Length; ++i)
        //{
        //    for (int j = 0; j < _buttons.Length; ++j)
        //    {
        //        if (i == j)
        //        {
        //            continue;
        //        }

        //        _buttons[i].OnHoverForOtherButton -= _buttons[j].DeActivateHoveredFrame;
        //        _buttons[i].OnHoverForOtherButton += _buttons[j].DeActivateHoveredFrame;
        //    }

        //    _buttons[i].OnHoverForController -= GetHoveredButtonIndex;
        //    _buttons[i].OnHoverForController += GetHoveredButtonIndex;

        //    _buttons[i].OnClickForController -= TriggerEventByClick;
        //    _buttons[i].OnClickForController += TriggerEventByClick;
        //}

        OnSelect -= PresenterManager.TriggerUIPresenter.DeActivateUI;
        OnSelect += PresenterManager.TriggerUIPresenter.DeActivateUI;
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
            while (_delayTime < 1)
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

                if (upKey && _hoveredButtonIndex != (int)BoxButtonID.Take)
                {
                    _hoveredButtonIndex = (int)BoxButtonID.Take;
                    _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                    SoundPool.GetPlayAudio(SoundID.ButtonMove);
                }
                else if (false == upKey && _hoveredButtonIndex != (int)BoxButtonID.Drop)
                {
                    _hoveredButtonIndex = (int)BoxButtonID.Drop;
                    _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                    SoundPool.GetPlayAudio(SoundID.ButtonMove);
                }
            }

            yield return null;
        }
    }
    private void GetHoveredButtonIndex(MyButton button)
    {
        SoundPool.GetPlayAudio(SoundID.ButtonMove);
        if (button == _buttons[(int)BoxButtonID.Take])
        {
            _hoveredButtonIndex = (int)BoxButtonID.Take;
        }
        else
        {
            _hoveredButtonIndex = (int)BoxButtonID.Drop;
        }
    }
    private void TriggerEventByKey() => ButtonSelect((BoxButtonID)_hoveredButtonIndex);
    private void TriggerEventByClick(MyButton button) => ButtonSelect((BoxButtonID)_hoveredButtonIndex);
    private void ButtonSelect(BoxButtonID ID)
    {
        StopCoroutine(_getKeyCoroutine);

        SoundPool.GetPlayAudio(SoundID.ButtonClick);

        if (ID == BoxButtonID.Take)
        {
            OnSelectTake?.Invoke();
        }

        OnSelect?.Invoke();
    }
}
