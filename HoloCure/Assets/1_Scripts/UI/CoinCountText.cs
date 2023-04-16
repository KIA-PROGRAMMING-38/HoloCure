using StringLiterals;
using TMPro;

public class CountText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.CountPresenter.OnUpdateCoinCount -= UpdateCoin;
        PresenterManager.CountPresenter.OnUpdateCoinCount += UpdateCoin;
    }
    private void UpdateCoin(int level) => _text.text = NumLiteral.GetNumString(level);
}
