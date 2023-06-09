using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
public enum PauseButtonID
{
    Skill,
    Stamp,
    Collab,
    Resume,
    Setting,
    Quit
}
public class PauseButtonController : UIBase
{
    private Action[] _onSelectedEvents;
    public event Action OnSkillSelected;
    public event Action OnStampSelected;
    public event Action OnCollabSelected;
    public event Action OnResumeSelected;
    public event Action OnSettingSelected;
    public event Action OnQuitSelected;

    private Action[] _onCanceledEvents;
    public event Action OnSkillCanceled;
    public event Action OnStampCanceled;
    public event Action OnCollabCanceled;
    public event Action OnSettingCanceled;
    public event Action OnQuitCanceled;

    [SerializeField] private MyButton[] _buttons;
    private int _hoveredButtonIndex;

    [SerializeField] private Canvas[] _UIs;
    private PauseButtonID _curUI;

    [SerializeField] private QuitButtonController _quitController;

    public void InitializeEventArray()
    {
        OnQuitSelected -= StopGetKeyCoroutine;
        OnQuitSelected += StopGetKeyCoroutine;
        OnQuitSelected -= _quitController.StartGetKeyCoroutine;
        OnQuitSelected += _quitController.StartGetKeyCoroutine;

        _onSelectedEvents = new Action[6];
        _onSelectedEvents[(int)PauseButtonID.Skill] = OnSkillSelected;
        _onSelectedEvents[(int)PauseButtonID.Stamp] = OnStampSelected;
        _onSelectedEvents[(int)PauseButtonID.Collab] = OnCollabSelected;
        _onSelectedEvents[(int)PauseButtonID.Resume] = OnResumeSelected;
        _onSelectedEvents[(int)PauseButtonID.Setting] = OnSettingSelected;
        _onSelectedEvents[(int)PauseButtonID.Quit] = OnQuitSelected;


        OnQuitCanceled -= _quitController.StopGetKeyCoroutine;
        OnQuitCanceled += _quitController.StopGetKeyCoroutine;
        OnQuitCanceled -= StartGetKeyCoroutine;
        OnQuitCanceled += StartGetKeyCoroutine;

        _onCanceledEvents = new Action[6];
        _onCanceledEvents[(int)PauseButtonID.Skill] = OnSkillCanceled;
        _onCanceledEvents[(int)PauseButtonID.Stamp] = OnStampCanceled;
        _onCanceledEvents[(int)PauseButtonID.Collab] = OnCollabCanceled;
        _onCanceledEvents[(int)PauseButtonID.Resume] = null;
        _onCanceledEvents[(int)PauseButtonID.Setting] = OnSettingCanceled;
        _onCanceledEvents[(int)PauseButtonID.Quit] = OnQuitCanceled;

        _quitController.OnSelectNo -= ButtonCancel;
        _quitController.OnSelectNo += ButtonCancel;
    }
    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivatePauseUI -= StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivatePauseUI += StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivatePauseUI -= SetBoolIsMainPauseUIOnTrue;
        PresenterManager.TriggerUIPresenter.OnActivatePauseUI += SetBoolIsMainPauseUIOnTrue;

        PresenterManager.TriggerUIPresenter.OnResume -= SetBoolIsMainPauseUIOnFalse;
        PresenterManager.TriggerUIPresenter.OnResume += SetBoolIsMainPauseUIOnFalse;
        PresenterManager.TriggerUIPresenter.OnResume -= StopGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnResume += StopGetKeyCoroutine;

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

        OnResumeSelected -= PresenterManager.TriggerUIPresenter.DeActivateUI;
        OnResumeSelected += PresenterManager.TriggerUIPresenter.DeActivateUI;
    }
    private bool _isMainPauseUIOn;
    private void SetBoolIsMainPauseUIOnTrue() => _isMainPauseUIOn = true;
    private void SetBoolIsMainPauseUIOnFalse() => _isMainPauseUIOn = false;
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

            if (_isMainPauseUIOn)
            {
                if (Input.GetButtonDown(InputLiteral.CONFIRM))
                {
                    TriggerEventByKey();
                    yield return null;
                }
                else if (Input.GetButtonDown(InputLiteral.VERTICAL))
                {
                    bool upKey = Input.GetAxisRaw(InputLiteral.VERTICAL) == 1;

                    if (upKey && _hoveredButtonIndex != (int)PauseButtonID.Skill)
                    {
                        _hoveredButtonIndex -= 1;
                        _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                        SoundPool.GetPlayAudio(SoundID.ButtonMove);
                    }
                    else if (false == upKey && _hoveredButtonIndex != (int)PauseButtonID.Quit)
                    {
                        _hoveredButtonIndex += 1;
                        _buttons[_hoveredButtonIndex].ActivateHoveredFrame();
                        SoundPool.GetPlayAudio(SoundID.ButtonMove);
                    }
                }
                else if (Input.GetButtonDown(InputLiteral.CANCEL))
                {
                    SoundPool.GetPlayAudio(SoundID.ButtonBack);
                    OnResumeSelected?.Invoke();
                    yield return null;
                }
            }
            else
            {
                if (Input.GetButtonDown(InputLiteral.CANCEL))
                {
                    ButtonCancel(_curUI);
                    yield return null;
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
        SoundPool.GetPlayAudio(SoundID.ButtonMove);
    }
    private void TriggerEventByKey() => ButtonSelect((PauseButtonID)_hoveredButtonIndex);
    private void TriggerEventByClick(MyButton button) => ButtonSelect((PauseButtonID)_hoveredButtonIndex);
    private void ButtonSelect(PauseButtonID ID)
    {
        SoundPool.GetPlayAudio(SoundID.ButtonClick);
        if (ID == PauseButtonID.Setting)
        {
            return;
        }

        SetBoolIsMainPauseUIOnFalse();
        _onSelectedEvents[(int)ID]?.Invoke();
        if (ID == PauseButtonID.Resume)
        {
            return;
        }
        _UIs[(int)ID].enabled = true;
        _curUI = ID;
    }
    private void ButtonCancel(PauseButtonID ID)
    {
        if (ID == PauseButtonID.Resume || ID == PauseButtonID.Setting)
        {
            return;
        }
        SoundPool.GetPlayAudio(SoundID.ButtonBack);
        SetBoolIsMainPauseUIOnTrue();
        _onCanceledEvents[(int)ID]?.Invoke();
        _UIs[(int)ID].enabled = false;
    }
}