using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
public enum ModeID
{
    Stage = 0,
    Endless = 1
}

public class SelectModeController : MonoBehaviour
{
    public event Action OnSelectStageMode;
    public event Action<ModeID> OnSelectStageModeToUI;

    public event Action OnCancel;

    [SerializeField] private Transform _cursor;
    [SerializeField] private MyFlashButton[] _modes;
    private int _hoveredButtonIndex;
    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();        

        for (int i = 0; i < _modes.Length; ++i)
        {
            _modes[i].OnHoverForController -= GetHoveredButtonIndex;
            _modes[i].OnHoverForController += GetHoveredButtonIndex;

            _modes[i].OnClickForController -= TriggerEventByClick;
            _modes[i].OnClickForController += TriggerEventByClick;
        }

        gameObject.SetActive(false);
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

                if (upKey && _hoveredButtonIndex != (int)ModeID.Stage)
                {
                    _hoveredButtonIndex = (int)ModeID.Stage;
                    _cursor.SetParent(_modes[_hoveredButtonIndex].transform, false);
                }
                else if (false == upKey && _hoveredButtonIndex != (int)ModeID.Endless)
                {
                    _hoveredButtonIndex = (int)ModeID.Endless;
                    _cursor.SetParent(_modes[_hoveredButtonIndex].transform, false);
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
    private void GetHoveredButtonIndex(MyFlashButton button)
    {
        _cursor.SetParent(button.transform, false);
        if (button == _modes[(int)ModeID.Stage])
        {
            _hoveredButtonIndex = (int)ModeID.Stage;
        }
        else
        {
            _hoveredButtonIndex = (int)ModeID.Endless;
        }
    }
    private void TriggerEventByKey() => ButtonSelect((ModeID)_hoveredButtonIndex);
    private void TriggerEventByClick(MyFlashButton button)
    {
        GetHoveredButtonIndex(button);
        ButtonSelect((ModeID)_hoveredButtonIndex);
    }
    private void ButtonSelect(ModeID ID)
    {

        if (ID == ModeID.Stage)
        {
        StopGetKeyCoroutine();
            OnSelectStageMode?.Invoke();
            OnSelectStageModeToUI?.Invoke(ID);
        }
        else
        {
            // Endless Mode ¹Ì±¸Çö
        }
    }
    private void Cancel()
    {
        StopGetKeyCoroutine();
        OnCancel?.Invoke();
    }
}
