using UnityEngine;
using UnityEngine.UI;

public class FlashMode : MonoBehaviour
{
    private Image _image;
    private void Awake() => _image = GetComponent<Image>();
    private readonly Color START_COLOR = new(1, 1, 1, 0);
    private readonly Color FLASH_COLOR = new(1, 1, 1, 0.3f);
    private float _elapsedTime;
    private const int FLASH_SPEED = 8;
    private void Update()
    {
        _image.color = Color.Lerp(START_COLOR, FLASH_COLOR, (Mathf.Sin(_elapsedTime * FLASH_SPEED) + 1) / 2);
        _elapsedTime += Time.unscaledDeltaTime;
    }
}
