using UniRx;

public class HUDTimerPresenter : Presenter
{
    private HUDTimerView _timerView;
    protected override void InitView()
    {
        _timerView = transform.FindAssert("HUD Timer View").GetComponentAssert<HUDTimerView>();
    }
    protected override void OnRelease()
    {
        _timerView = default;
    }
    protected override void OnUpdatedModel()
    {
        Managers.Game.Seconds.Subscribe(UpdateSecondsText).AddTo(CompositeDisposable);
        Managers.Game.Minutes.Subscribe(UpdateMinutesText).AddTo(CompositeDisposable);
    }
    private void UpdateSecondsText(int seconds)
    {
        _timerView.SecondsText.text = seconds.ToString("D2");
    }

    private void UpdateMinutesText(int minutes)
    {
        _timerView.MinutesText.text = minutes.ToString("D2");
    }
}