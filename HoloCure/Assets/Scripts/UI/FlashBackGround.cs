using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class FlashBackGround : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private float _elapsedTime;
    private const float FLASH_TIME = 1.5f;
    private void Awake() => _canvasGroup = gameObject.GetComponentAssert<CanvasGroup>();

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(Flash);
    }

    private void Flash(Unit unit)
    {
        _canvasGroup.alpha = Mathf.Lerp(0.9f, 0, _elapsedTime / FLASH_TIME);
        _elapsedTime += Time.unscaledDeltaTime;
    }
}
