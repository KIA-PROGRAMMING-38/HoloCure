using StringLiterals;
using TMPro;

public class MaxHPText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.HPPresenter.OnUpdateMaxHP -= UpdateMaxHP;
        PresenterManager.HPPresenter.OnUpdateMaxHP += UpdateMaxHP;
    }
    private void UpdateMaxHP(int value) => _text.text = NumLiteral.GetNumString(value);
}
