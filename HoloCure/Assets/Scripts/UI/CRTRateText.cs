using StringLiterals;
using TMPro;

public class CRTRateText : UIBaseLegacy
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.StatPresenter.OnUpdateCRT -= UpdateCRT;
        PresenterManager.StatPresenter.OnUpdateCRT += UpdateCRT;
    }
    private void UpdateCRT(int value) => _text.text = NumLiteral.GetNumString(value);
}
