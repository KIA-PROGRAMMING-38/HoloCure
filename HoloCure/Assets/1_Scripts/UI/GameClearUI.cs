using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUI : UIBase
{
    [SerializeField] private Image _backGround;
    [SerializeField] private RectTransform _gameClearText;
    [SerializeField] private GameClearButtonController _buttons;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        _startPos = _gameClearText.anchoredPosition + Vector2.up * 500;
        _endPos = _gameClearText.anchoredPosition;
        _lerpCoroutine = LerpCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateGameClearUI -= ActivateGameClearUI;
        PresenterManager.TriggerUIPresenter.OnActivateGameClearUI += ActivateGameClearUI;

        PresenterManager.TriggerUIPresenter.OnGameEnd -= DeActivateGameClearUI;
        PresenterManager.TriggerUIPresenter.OnGameEnd += DeActivateGameClearUI;
    }
    private void ActivateGameClearUI()
    {
        SoundPool.GetPlayAudio(SoundID.GameClear);

        _canvas.enabled = true;
        _elapsedTime = 0;
        _backGround.color = START_COLOR;
        _gameClearText.anchoredPosition = _startPos;
        StartCoroutine(_lerpCoroutine);
    }
    private void DeActivateGameClearUI() => _canvas.enabled = false;

    private readonly Color START_COLOR = new(1, 1, 1, 0);
    private readonly Color END_COLOR = new(1, 1, 1, 0.5f);
    private Vector2 _startPos;
    private Vector2 _endPos;
    private const int GAME_CLEAR_TIME = 7;
    private float _elapsedTime;
    private IEnumerator _lerpCoroutine;
    private IEnumerator LerpCoroutine()
    {
        while (true)
        {
            while (_elapsedTime < GAME_CLEAR_TIME)
            {
                _backGround.color = Color.Lerp(START_COLOR, END_COLOR, _elapsedTime / GAME_CLEAR_TIME);
                _gameClearText.anchoredPosition = Vector2.Lerp(_startPos, _endPos, _elapsedTime / GAME_CLEAR_TIME);
                _elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            StopCoroutine(_lerpCoroutine);

            _buttons.gameObject.SetActive(true);
            _buttons.StartGetKeyCoroutine();

            yield return null;
        }
    }
}
