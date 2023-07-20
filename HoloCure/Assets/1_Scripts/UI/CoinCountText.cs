using StringLiterals;
using TMPro;

public class CoinCountText : UIBaseLegacy
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.CountPresenter.OnUpdateCoinCount -= UpdateCoin;
        PresenterManager.CountPresenter.OnUpdateCoinCount += UpdateCoin;
    }
    private void UpdateCoin(int coin) => _text.text = NumLiteral.GetNumString(coin);
}
