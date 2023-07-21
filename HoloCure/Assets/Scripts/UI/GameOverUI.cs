using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : UIBaseLegacy
{
    [SerializeField] private Image _backGround;
    [SerializeField] private RectTransform _gameOverText;
    [SerializeField] private GameOverButtonController _buttons;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        _startPos = _gameOverText.anchoredPosition + Vector2.up * 500;
        _endPos = _gameOverText.anchoredPosition;
        _lerpCoroutine = LerpCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateGameOverUI -= ActivateGameOverUI;
        PresenterManager.TriggerUIPresenter.OnActivateGameOverUI += ActivateGameOverUI;

        PresenterManager.TriggerUIPresenter.OnGameEnd -= DeActivateGameOverUI;
        PresenterManager.TriggerUIPresenter.OnGameEnd += DeActivateGameOverUI;
    }
    private void ActivateGameOverUI()
    {
        SoundPool.GetPlayAudio(SoundID.GameOver);

        _canvas.enabled = true;
        _elapsedTime = 0;
        _backGround.color = START_COLOR;
        _gameOverText.anchoredPosition = _startPos;
        StartCoroutine(_lerpCoroutine);
    }
    private void DeActivateGameOverUI() => _canvas.enabled = false;

    private readonly Color START_COLOR = new(1, 1, 1, 0);
    private readonly Color END_COLOR = new(1, 1, 1, 0.5f);
    private Vector2 _startPos;
    private Vector2 _endPos;
    private const int GAME_OVER_TIME = 5;
    private float _elapsedTime;
    private IEnumerator _lerpCoroutine;
    private IEnumerator LerpCoroutine()
    {
        while (true)
        {
            while (_elapsedTime < GAME_OVER_TIME)
            {
                _backGround.color = Color.Lerp(START_COLOR, END_COLOR, _elapsedTime / GAME_OVER_TIME);
                _gameOverText.anchoredPosition = Vector2.Lerp(_startPos, _endPos, _elapsedTime / GAME_OVER_TIME);
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
