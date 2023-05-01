using System.Collections;
using UnityEngine;

public class MouseCursor : UIBase
{
    [SerializeField] private RectTransform _cursorUI;
    [SerializeField] private RectTransform _cusorInGame;
    private Vector2 _cusorUIInitPos;
    private Vector2 _cusorInGameInitPos;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        _cusorUIInitPos = _cursorUI.anchoredPosition;
        _cusorInGameInitPos = _cusorInGame.anchoredPosition;

        _UICursorMoveCoroutine = UICursorMoveCoroutine();
        _InGameCursorMoveCoroutine = InGameCursorMoveCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateDefaultUI -= ActivateUICursor;
        PresenterManager.TriggerUIPresenter.OnActivateDefaultUI += ActivateUICursor;
        PresenterManager.TriggerUIPresenter.OnActivateGameOverUI -= ActivateUICursor;
        PresenterManager.TriggerUIPresenter.OnActivateGameOverUI += ActivateUICursor;
        PresenterManager.TriggerUIPresenter.OnActivateGameClearUI -= ActivateUICursor;
        PresenterManager.TriggerUIPresenter.OnActivateGameClearUI += ActivateUICursor;


        PresenterManager.TriggerUIPresenter.OnResume -= ActivateInGameCursor;
        PresenterManager.TriggerUIPresenter.OnResume += ActivateInGameCursor;

        ActivateUICursor();
    }
    private void ActivateUICursor()
    {
        StopCoroutine(_InGameCursorMoveCoroutine);
        _cusorInGame.gameObject.SetActive(false);
        _cursorUI.gameObject.SetActive(true);
        StartCoroutine(UICursorMoveCoroutine());
    }
    private void ActivateInGameCursor()
    {
        StopCoroutine(_UICursorMoveCoroutine);
        _cursorUI.gameObject.SetActive(false);
        _cusorInGame.gameObject.SetActive(true);
        StartCoroutine(_InGameCursorMoveCoroutine);
    }
    private IEnumerator _UICursorMoveCoroutine;
    private IEnumerator UICursorMoveCoroutine()
    {
        while (true)
        {
            _cursorUI.anchoredPosition = _cusorUIInitPos + Util.Caching.MouseScreenPos;

            yield return null;
        }
    }
    private IEnumerator _InGameCursorMoveCoroutine;
    private IEnumerator InGameCursorMoveCoroutine()
    {
        while (true)
        {
            _cusorInGame.anchoredPosition = _cusorInGameInitPos + Util.Caching.MouseScreenPos;

            yield return null;
        }
    }
}
