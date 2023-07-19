using StringLiterals;
using TMPro;

public class MinuteText : UIBaseLegacy
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.TimePresenter.OnUpdateMinute -= UpdateMinute;
        PresenterManager.TimePresenter.OnUpdateMinute += UpdateMinute;
    }
    private void UpdateMinute(int minute) => _text.text = NumLiteral.GetTimeNumString(minute);
}
