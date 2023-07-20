using StringLiterals;
using TMPro;

public class PlayerLevelText : UIBaseLegacy
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.CountPresenter.OnUpdatePlayerLevelCount -= UpdatePlayerLevel;
        PresenterManager.CountPresenter.OnUpdatePlayerLevelCount += UpdatePlayerLevel;
    }
    private void UpdatePlayerLevel(int level) => _text.text = NumLiteral.GetNumString(level);
}