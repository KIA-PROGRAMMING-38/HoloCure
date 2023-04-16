using StringLiterals;
using TMPro;

public class CurHPText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.HPPresenter.OnUpdateCurHP -= UpdateCurHP;
        PresenterManager.HPPresenter.OnUpdateCurHP += UpdateCurHP;
    }
    private void UpdateCurHP(int value) => _text.text = NumLiteral.GetNumString(value);
}
