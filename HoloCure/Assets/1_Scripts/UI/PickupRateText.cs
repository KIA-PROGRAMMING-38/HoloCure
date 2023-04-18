using StringLiterals;
using TMPro;

public class PickupRateText : UIBase
{
    private TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        PresenterManager.StatPresenter.OnUpdatePickup -= UpdatePickup;
        PresenterManager.StatPresenter.OnUpdatePickup += UpdatePickup;
    }
    private void UpdatePickup(int value) => _text.text = NumLiteral.GetNumString(value);
}
