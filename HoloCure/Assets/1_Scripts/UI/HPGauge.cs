using UnityEngine.UI;

public class HPGauge : UIBase
{
    private Image _image;
    private void Awake() => _image = GetComponent<Image>();
    private void Start()
    {
        PresenterManager.HPPresenter.OnUpdateHPGauge -= UpdateHPGauge;
        PresenterManager.HPPresenter.OnUpdateHPGauge += UpdateHPGauge;
    }
    private void UpdateHPGauge(float hpRate) => _image.fillAmount = hpRate;
}
