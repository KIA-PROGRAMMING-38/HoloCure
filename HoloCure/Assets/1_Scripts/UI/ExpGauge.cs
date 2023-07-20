using UnityEngine.UI;

public class ExpGauge : UIBaseLegacy
{
    private Image _image;
    private void Awake() => _image = GetComponent<Image>();
    private void Start()
    {
        PresenterManager.ExpPresenter.OnUpdateExpGauge -= UpdateExpGauge;
        PresenterManager.ExpPresenter.OnUpdateExpGauge += UpdateExpGauge;
    }
    private void UpdateExpGauge(float expRate) => _image.fillAmount = expRate;
}