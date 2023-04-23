using TMPro;
using UnityEngine;

public class ExclamationMark : MonoBehaviour
{
    private DamageText _damageText;
    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _damageText = transform.parent.GetComponent<DamageText>();

        _damageText.OnMove -= SetPosition;
        _damageText.OnMove += SetPosition;
        _damageText.OnFade -= Fade;
        _damageText.OnFade += Fade;
        _damageText.OnInitialize -= ResetColor;
        _damageText.OnInitialize += ResetColor;
    }
    private const int OFFSET_VALUE = 5;
    private void SetPosition(Vector2 pos) => transform.position = pos + Vector2.right * OFFSET_VALUE;
    private void Fade(Color color) => _text.color = color;
    private void ResetColor(Color color) => _text.color = color;
}