using StringLiterals;
using TMPro;

public class DefeatedEnemyCountText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.CountPresenter.OnUpdateDefeatedEnemyCount -= UpdateDefeatedEnemy;
        PresenterManager.CountPresenter.OnUpdateDefeatedEnemyCount += UpdateDefeatedEnemy;
    }
    private void UpdateDefeatedEnemy(int level) => _text.text = NumLiteral.GetNumString(level);
}
