using StringLiterals;
using TMPro;

public class ATKRateText : UIBaseLegacy
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.StatPresenter.OnUpdateATK -= UpdateATK;
        PresenterManager.StatPresenter.OnUpdateATK += UpdateATK;
    }
    private void UpdateATK(int value) => _text.text = NumLiteral.GetNumString(value);
}
