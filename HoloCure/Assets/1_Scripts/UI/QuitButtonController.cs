using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
public enum QuitButtonID
{
    Yes,
    No
}
public class QuitButtonController : MonoBehaviour
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
    }
    public void StartGetKeyCoroutine() => StartCoroutine(_getKeyCoroutine);
    public void StopGetKeyCoroutine() => StopCoroutine(_getKeyCoroutine);
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

                if (upKey && _hoveredButtonIndex != (int)QuitButtonID.Yes)
                {
                    _hoveredButtonIndex = (int)QuitButtonID.Yes;
                    _buttons[_hoveredButtonIndex].HoveredByKey();
                }
                else if (false == upKey && _hoveredButtonIndex != (int)QuitButtonID.No)
                {
                    _hoveredButtonIndex = (int)QuitButtonID.No;
                    _buttons[_hoveredButtonIndex].HoveredByKey();
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
    private void TriggerEventByClick(MyButton button)
    {
        GetHoveredButtonIndex(button);
        ButtonSelect((QuitButtonID)_hoveredButtonIndex);
    }
    private void ButtonSelect(QuitButtonID ID)
    {
        if (ID == QuitButtonID.Yes)
        {
            OnSelectYes?.Invoke();
        }
        else
        {
            OnSelectNo?.Invoke(PauseButtonID.Quit);
        }
    }
}
