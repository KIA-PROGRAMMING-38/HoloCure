using UnityEngine.UI;

public class ExpGauge : UIBase
{
    private Image _image;
    private void Awake() => _image = GetComponent<Image>();
    private void Start()
    {
        PresenterManager.ExpPresenter.OnUpdateExpGauge -= UpdateExpGauge;
        PresenterManager.ExpPresenter.OnUpdateExpGauge += UpdateExpGauge;
    }
    private void UpdateExpGauge(float expRate)
    {
        UnityEngine.Debug.Log(expRate);
        _image.fillAmount = expRate;
    }
}