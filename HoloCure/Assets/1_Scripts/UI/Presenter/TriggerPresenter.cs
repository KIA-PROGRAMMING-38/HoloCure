using System;

public class TriggerPresenter
{
    public event Action OnPause;
    public event Action OnResume;
    public event Action OnLevelUp;

    public void ActivateStatUI()
    {
        OnPause?.Invoke();
    }
    public void DeActivateStatUI()
    {
        OnResume?.Invoke();
    }
}