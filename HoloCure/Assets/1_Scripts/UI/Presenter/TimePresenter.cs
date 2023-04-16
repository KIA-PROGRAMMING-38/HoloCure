using System;

public class TimePresenter
{
    public event Action<int> OnUpdateMinute;
    public event Action<int> OnUpdateSecond;

    private int _curMinute;
    private int _curSecond;

    private void UpdateMinute() => OnUpdateMinute?.Invoke(_curMinute);
    private void UpdateSecond() => OnUpdateSecond?.Invoke(_curSecond);
    public void IncreaseOneSecond()
    {
        _curSecond += 1;
        if (_curSecond == 60)
        {
            _curSecond -= 60;
            _curMinute += 1;
            UpdateMinute();
        }
        UpdateSecond();
    }
}