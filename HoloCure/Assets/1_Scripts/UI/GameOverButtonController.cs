using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class GameOverButtonController : UIBaseLegacy
{
    public event Action OnSelectMainMenu;

    //[SerializeField] private MyButton _button;
    private int _hoveredButtonIndex = 0;

    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        //_button.OnClickForController -= TriggerEventByClick;
        //_button.OnClickForController += TriggerEventByClick;

        OnSelectMainMenu -= PresenterManager.TriggerUIPresenter.GameEnd;
        OnSelectMainMenu += PresenterManager.TriggerUIPresenter.GameEnd;
         
        gameObject.SetActive(false);
    }
    public void StartGetKeyCoroutine()
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

            yield return null;
        }
    }

    private void TriggerEventByKey() => ButtonSelect(_hoveredButtonIndex);
    //private void TriggerEventByClick(MyButton button) => ButtonSelect(_hoveredButtonIndex);
    private void ButtonSelect(int ID)
    {
        StopCoroutine(_getKeyCoroutine);
        SoundPool.GetPlayAudio(SoundID.CharClick);
        OnSelectMainMenu?.Invoke();

        gameObject.SetActive(false);
    }
}
