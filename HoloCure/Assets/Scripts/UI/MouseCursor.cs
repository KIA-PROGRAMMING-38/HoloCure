using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class MouseCursor : UIBase
{
    private RectTransform _uiCursor;
    private RectTransform _ingameCursor;

    private Vector2 _uiOffset;
    private Vector2 _ingameOffset;

    private float _previousTimeScale;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        _uiCursor = transform.FindAssert("UiCursor").GetComponentAssert<RectTransform>();
        _ingameCursor = transform.FindAssert("IngameCursor").GetComponentAssert<RectTransform>();

        _uiOffset = _uiCursor.anchoredPosition;
        _ingameOffset = _ingameCursor.anchoredPosition;

        _previousTimeScale = Time.timeScale;
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(UpdateCursor);
    }

    private void UpdateCursor(Unit unit)
    {
        if (Time.timeScale != _previousTimeScale)
        {
            if (Time.timeScale < 1)
            {
                ActivateUICursor();
            }
            else
            {
                ActivateInGameCursor();
            }
        }

        if (Time.timeScale < 1)
        {
            _uiCursor.anchoredPosition = _uiOffset + Util.CursorCache.MouseScreenPos;
        }
        else
        {
            _ingameCursor.anchoredPosition = _ingameOffset + Util.CursorCache.MouseScreenPos;
        }

        _previousTimeScale = Time.timeScale;
    }

    private void ActivateUICursor()
    {
        _ingameCursor.gameObject.SetActive(false);
        _uiCursor.gameObject.SetActive(true);
    }
    private void ActivateInGameCursor()
    {
        _uiCursor.gameObject.SetActive(false);
        _ingameCursor.gameObject.SetActive(true);
    }
}
