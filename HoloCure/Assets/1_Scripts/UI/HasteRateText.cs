using StringLiterals;
using TMPro;

public class HasteRateText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.StatPresenter.OnUpdateHaste -= UpdateHaste;
        PresenterManager.StatPresenter.OnUpdateHaste += UpdateHaste;
    }
    private void UpdateHaste(int value) => _text.text = NumLiteral.GetNumString(value);
}
