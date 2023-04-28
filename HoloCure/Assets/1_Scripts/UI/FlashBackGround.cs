using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashBackGround : MonoBehaviour
{
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _flashCoroutine = FlashCoroutine();
    }

    private readonly Color START_COLOR = new(1, 1, 1, 0.9f);
    private readonly Color END_COLOR = new(1, 1, 1, 0);
    private void OnEnable()
    {
        _image.color = START_COLOR;
        _flashTime = 0;
        StartCoroutine(_flashCoroutine);
    }

    private float _flashTime;
    private IEnumerator _flashCoroutine;
    private IEnumerator FlashCoroutine()
    {
        while (true)
        {
            while (_flashTime < 1.5f)
            {
                _image.color = Color.Lerp(START_COLOR, END_COLOR, _flashTime / 1.5f);
                _flashTime += Time.unscaledDeltaTime;
                yield return null;
            }

            StopCoroutine(_flashCoroutine);

            yield return null;
        }
    }
}
