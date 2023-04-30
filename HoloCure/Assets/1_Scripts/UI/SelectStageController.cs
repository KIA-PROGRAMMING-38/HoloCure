using StringLiterals;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum StageID
{
    GrassyPlains = 0,
    HoloOffice = 1,
    HalloweenCastle = 2
}
public class SelectStageController : MonoBehaviour
{
    public event Action OnPlay;
    public event Action<StageID> OnPlayToUI;

    public event Action OnCancel;

    [SerializeField] private TextMeshProUGUI _stageNumText;
    [SerializeField] private Image _stageImage;
    [SerializeField] private TextMeshProUGUI _stageNameText;

    [SerializeField] private Sprite[] _stageSprites;
    [SerializeField] private string[] _stageNames;

    [SerializeField] private MyFlashButton[] _buttons;
    private Image _stageButtonImage;
    private int _curStageIndex;
    private enum StageButtonID { Stage, Left, Right, Play }
    private void Awake() => _stageButtonImage = _buttons[(int)StageButtonID.Stage].GetComponent<Image>();

    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        for (int i = 0; i < _buttons.Length; ++i)
        {
            _buttons[i].OnClickForController -= TriggerEventByClick;
            _buttons[i].OnClickForController += TriggerEventByClick;
        }

        gameObject.SetActive(false);
    }
    private bool _isPlayButtonOn;
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

            if (false == _isPlayButtonOn)
            {
                if (Input.GetButtonDown(InputLiteral.CONFIRM))
                {
                    ButtonSelect(StageButtonID.Stage);
                    yield return null;
                }
                else if (Input.GetButtonDown(InputLiteral.HORIZONTAL))
                {
                    bool rightKey = Input.GetAxisRaw(InputLiteral.HORIZONTAL) == 1;

                    if (rightKey)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                }
                else if (Input.GetButtonDown(InputLiteral.CANCEL))
                {
                    CancelSelectStage();
                    yield return null;
                }
            }
            else
            {
                if (Input.GetButtonDown(InputLiteral.CONFIRM))
                {
                    ButtonSelect(StageButtonID.Play);
                    yield return null;
                }
                else if (Input.GetButtonDown(InputLiteral.CANCEL))
                {
                    CancelStartStage();
                    yield return null;
                }
            }
            yield return null;
        }
    }
    private void TriggerEventByClick(MyFlashButton button)
    {
        int buttonIndex = 0;

        for (int i = 0; i < _buttons.Length; ++i)
        {
            if (button != _buttons[i])
            {
                continue;
            }

            buttonIndex = i;
            break;
        }

        ButtonSelect((StageButtonID)buttonIndex);
    }
    private void ButtonSelect(StageButtonID ID)
    {
        switch (ID)
        {
            case StageButtonID.Stage:
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    return;
                }
                SelectStage();
                break;
            case StageButtonID.Left:
                MoveLeft();
                break;
            case StageButtonID.Right:
                MoveRight();
                break;
            case StageButtonID.Play:
                StartStage();
                break;
        }
    }
    private void MoveLeft()
    {
        _curStageIndex -= 1;
        if (_curStageIndex < (int)StageID.GrassyPlains)
        {
            _curStageIndex = (int)StageID.HalloweenCastle;
        }

        StageChange();
    }
    private void MoveRight()
    {
        _curStageIndex += 1;
        if (_curStageIndex > (int)StageID.HalloweenCastle)
        {
            _curStageIndex = (int)StageID.GrassyPlains;
        }

        StageChange();
    }
    private readonly Color UNLOCK = new(1, 1, 1, 0);
    private readonly Color LOCK = new(1, 1, 1, 1);
    private void StageChange()
    {
        _stageImage.sprite = _stageSprites[_curStageIndex];
        _stageNameText.text = _stageNames[_curStageIndex];
        _stageNumText.text = NumLiteral.GetNumString(_curStageIndex + 1);
        if (_curStageIndex == (int)StageID.GrassyPlains)
        {
            _stageButtonImage.color = UNLOCK;
        }
        else
        {
            _stageButtonImage.color = LOCK;
        }
    }
    private void SelectStage()
    {
        if (_curStageIndex != (int)StageID.GrassyPlains)
        {
            return;
        }
        _isPlayButtonOn = true;
        _buttons[(int)StageButtonID.Play].gameObject.SetActive(true);
    }
    private void StartStage()
    {
        StopGetKeyCoroutine();

        _isPlayButtonOn = false;
        _buttons[(int)StageButtonID.Play].gameObject.SetActive(false);

        OnPlayToUI?.Invoke((StageID)_curStageIndex);
        OnPlay?.Invoke();
    }
    private void CancelSelectStage()
    {
        StopGetKeyCoroutine();
        OnCancel?.Invoke();
    }
    private void CancelStartStage()
    {
        _isPlayButtonOn = false;
        _buttons[(int)StageButtonID.Play].gameObject.SetActive(false);
    }
}
