using StringLiterals;
using TMPro;

public class SecondText : UIBaseLegacy
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.TimePresenter.OnUpdateSecond -= UpdateSecond;
        PresenterManager.TimePresenter.OnUpdateSecond += UpdateSecond;
    }
    private void UpdateSecond(int second) => _text.text = NumLiteral.GetTimeNumString(second);
}
