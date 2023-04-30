using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
public enum MainTitleButtonID
{
    Play,
    Shop,
    Leaderboard,
    Achievements,
    Settings,
    Credits,
    Quit
}

public class TitleButtonController : UIBase
{
    public event Action OnResetBackGround;
    public event Action OnSelectPlay;

    [SerializeField] private MyButton[] _buttons;
    private int _hoveredButtonIndex;

    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI -= StartGetKeyCoroutine;
        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI += StartGetKeyCoroutine;

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

        OnResetBackGround -= PresenterManager.TitleUIPresenter.ResetTitleBackGroundUI;
        OnResetBackGround += PresenterManager.TitleUIPresenter.ResetTitleBackGroundUI;

        OnSelectPlay -= PresenterManager.TitleUIPresenter.ActivateSelectUI;
        OnSelectPlay += PresenterManager.TitleUIPresenter.ActivateSelectUI;

        StartCoroutine(_getKeyCoroutine);
    }
    private void StartGetKeyCoroutine()
    {
        _delayTime = 0;
        StopCoroutine(_getKeyCoroutine);
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

                if (upKey && _hoveredButtonIndex != (int)MainTitleButtonID.Play)
                {
                    _hoveredButtonIndex -= 1;
                    _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                }
                else if (false == upKey && _hoveredButtonIndex != (int)MainTitleButtonID.Quit)
                {
                    _hoveredButtonIndex += 1;
                    _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                }
            }

            yield return null;
        }
    }
    private void GetHoveredButtonIndex(MyButton button)
    {
        for (int i = 0; i < _buttons.Length; ++i)
        {
            if (button != _buttons[i])
            {
                continue;
            }

            _hoveredButtonIndex = i;
            break;
        }
    }
    private void TriggerEventByKey() => ButtonSelect((MainTitleButtonID)_hoveredButtonIndex);
    private void TriggerEventByClick(MyButton button)
    {
        GetHoveredButtonIndex(button);
        ButtonSelect((MainTitleButtonID)_hoveredButtonIndex);
    }    
    private void ButtonSelect(MainTitleButtonID ID)
    {
        switch (ID)
        {
            case MainTitleButtonID.Play:
                StopGetKeyCoroutine();
                OnResetBackGround?.Invoke();
                OnSelectPlay?.Invoke();
                break;
            case MainTitleButtonID.Shop:
                // �̱��� ���
                break;
            case MainTitleButtonID.Leaderboard:
                // �̱��� ���
                break;
            case MainTitleButtonID.Achievements:
                // �̱��� ���
                break;
            case MainTitleButtonID.Settings:
                // �̱��� ���
                break;
            case MainTitleButtonID.Credits:
                // �̱��� ���
                break;
            case MainTitleButtonID.Quit:
                StopGetKeyCoroutine();
                Application.Quit();
                break;
        }
    }
    // �̱��� ��� �κ��� ���� �����ϰ� �ȴٸ� PauseButtonControlleró�� Switch�� �Ⱦ��� �籸��
}
