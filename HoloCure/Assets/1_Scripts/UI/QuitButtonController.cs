using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
public enum QuitButtonID
{
    Yes = 0,
    No = 1
}
public class QuitButtonController : UIBaseLegacy
{
    public event Action OnSelectYes;
    public event Action<PauseButtonID> OnSelectNo;

    [SerializeField] private MyButton[] _buttons;
    private int _hoveredButtonIndex;

    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        for (int i = 0; i < _buttons.Length; ++i)
        {
            for (int j = 0; j < _buttons.Length; ++j)
            {
                if (i == j)
                {
                    continue;
                }

                _buttons[i].OnHoverForOtherButton -= _buttons[j].DeActivateHoveredFrame;
                _buttons[i].OnHoverForOtherButton += _buttons[j].DeActivateHoveredFrame;
            }

            _buttons[i].OnHoverForController -= GetHoveredButtonIndex;
            _buttons[i].OnHoverForController += GetHoveredButtonIndex;

            _buttons[i].OnClickForController -= TriggerEventByClick;
            _buttons[i].OnClickForController += TriggerEventByClick;
        }

        OnSelectYes -= PresenterManager.TriggerUIPresenter.GameEnd;
        OnSelectYes += PresenterManager.TriggerUIPresenter.GameEnd;
    }
    public void StartGetKeyCoroutine()
    {
        _delayTime = 0;
        StartCoroutine(_getKeyCoroutine);
    }
    public void StopGetKeyCoroutine() => StopCoroutine(_getKeyCoroutine);
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

                if (upKey && _hoveredButtonIndex != (int)QuitButtonID.Yes)
                {
                    _hoveredButtonIndex = (int)QuitButtonID.Yes;
                    _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                    SoundPool.GetPlayAudio(SoundID.ButtonMove);
                }
                else if (false == upKey && _hoveredButtonIndex != (int)QuitButtonID.No)
                {
                    _hoveredButtonIndex = (int)QuitButtonID.No;
                    _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                    SoundPool.GetPlayAudio(SoundID.ButtonMove);
                }
            }
            else if (Input.GetButtonDown(InputLiteral.CANCEL))
            {
                ButtonSelect(QuitButtonID.No);
                yield return null;
            }

            yield return null;
        }
    } 
    private void GetHoveredButtonIndex(MyButton button)
    {
        SoundPool.GetPlayAudio(SoundID.ButtonMove);
        if (button == _buttons[(int)QuitButtonID.Yes])
        {
            _hoveredButtonIndex = (int)QuitButtonID.Yes;
        }
        else
        {
            _hoveredButtonIndex = (int)QuitButtonID.No;
        }
    }
    private void TriggerEventByKey() => ButtonSelect((QuitButtonID)_hoveredButtonIndex);
    private void TriggerEventByClick(MyButton button) => ButtonSelect((QuitButtonID)_hoveredButtonIndex);
    private void ButtonSelect(QuitButtonID ID)
    {
        if (ID == QuitButtonID.Yes)
        {
            StopGetKeyCoroutine();
            SoundPool.GetPlayAudio(SoundID.ButtonClick);
            transform.parent.parent.GetComponent<Canvas>().enabled = false;
            OnSelectYes?.Invoke();
        }
        else
        {
            OnSelectNo?.Invoke(PauseButtonID.Quit);
        }
    }
}
