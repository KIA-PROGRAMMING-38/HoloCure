using StringLiterals;
using TMPro;

public class PlayerLevelText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.PlayerLevelPresenter.OnUpdatePlayerLevel -= UpdatePlayerLevel;
        PresenterManager.PlayerLevelPresenter.OnUpdatePlayerLevel += UpdatePlayerLevel;
    }
    private void UpdatePlayerLevel(int level)
    {
        _text.text = level switch
        {
            < 10  => DigitLiteral.DIGITS[level],
            < 100 => DigitLiteral.DIGITS[level / 10] + DigitLiteral.DIGITS[level % 10],
                _ => DigitLiteral.DIGITS[level / 100] + DigitLiteral.DIGITS[level / 10 % 10] + DigitLiteral.DIGITS[level % 10],
        };
    }
}