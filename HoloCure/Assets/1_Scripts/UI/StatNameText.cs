using TMPro;

public class StatNameText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.InitPresenter.OnUpdateName -= UpdateName;
        PresenterManager.InitPresenter.OnUpdateName += UpdateName;
    }
    private void UpdateName(string name) => _text.text = name;
}
