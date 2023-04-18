using StringLiterals;
using TMPro;

public class SPDRateText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.StatPresenter.OnUpdateSPD -= UpdateSPD;
        PresenterManager.StatPresenter.OnUpdateSPD += UpdateSPD;
    }
    private void UpdateSPD(int value) => _text.text = NumLiteral.GetNumString(value);
}
