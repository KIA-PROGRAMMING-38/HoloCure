using System;

public class ExpPresenter
{
    public event Action<float> OnUpdateExpGauge;
    public void UpdateExpGauge(float expRate) => OnUpdateExpGauge?.Invoke(expRate);
}