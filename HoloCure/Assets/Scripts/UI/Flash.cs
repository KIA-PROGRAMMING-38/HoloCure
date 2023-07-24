using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private const float START_ALPHA = 1.0f;
    private const float END_ALPHA = 0.3f;
    private const int FLASH_SPEED = 13;
    private float _elapsedTime;

    private CanvasGroup _canvasGroup;
    private void Awake() => _canvasGroup = gameObject.GetComponentAssert<CanvasGroup>();
    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(FlashCursor);
    }
    private void FlashCursor(Unit unit)
    {
        _canvasGroup.alpha = Mathf.Lerp(START_ALPHA, END_ALPHA, Mathf.Sin(_elapsedTime * FLASH_SPEED));
        _elapsedTime += Time.unscaledDeltaTime;
    }
    
}
