using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class OpenButtonController : UIBaseLegacy
{
    public event Action OnPress;

    //private MyButton _button;
    //private void Awake() => _button = GetComponent<MyButton>();
    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateGetBoxStartUI -= StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxStartUI += StartGetKeyCoroutine;

        OnPress -= PresenterManager.TriggerUIPresenter.ActivateGetBoxUI;
        OnPress += PresenterManager.TriggerUIPresenter.ActivateGetBoxUI;

        //_button.OnClick -= ButtonSelect;
        //_button.OnClick += ButtonSelect;
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

            if (Input.GetButtonDown(InputLiteral.CONFIRM) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                ButtonSelect();
                yield return null;
            }

            yield return null;
        }
    }
    private void ButtonSelect()
    {
        StopCoroutine(_getKeyCoroutine);
        OnPress?.Invoke();
    }
}
