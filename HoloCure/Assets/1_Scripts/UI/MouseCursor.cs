using System.Collections;
using UnityEngine;

public class MouseCursor : UIBase
{
    [SerializeField] private RectTransform _cursorUI;
    [SerializeField] private RectTransform _cusorInGame;
    private Vector2 _cusorUIInitPos;
    private Vector2 _cusorInGameInitPos;

    private void Awake() => Cursor.visible = false;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        _cusorUIInitPos = _cursorUI.anchoredPosition;
        _cusorInGameInitPos = _cusorInGame.anchoredPosition;

        _UICursorMoveCoroutine = UICursorMoveCoroutine();
        _InGameCursorMoveCoroutine = InGameCursorMoveCoroutine();

        PresenterManager.TriggerPresenter.OnPause -= ActivateUICursor;
        PresenterManager.TriggerPresenter.OnPause += ActivateUICursor;

        PresenterManager.TriggerPresenter.OnResume -= ActivateInGameCursor;
        PresenterManager.TriggerPresenter.OnResume += ActivateInGameCursor;

        StartCoroutine(_UICursorMoveCoroutine);
        StartCoroutine(_InGameCursorMoveCoroutine);
    }
    private void ActivateUICursor()
    {
        _cursorUI.gameObject.SetActive(true);
        _cusorInGame.gameObject.SetActive(false);
    }
    private void ActivateInGameCursor()
    {
        _cusorInGame.gameObject.SetActive(true);
        _cursorUI.gameObject.SetActive(false);
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
