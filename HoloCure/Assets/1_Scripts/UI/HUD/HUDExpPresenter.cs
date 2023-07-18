using UniRx;
public class HUDExpPresenter : Presenter
{
    private HUDExpView _expView;
    protected override void InitView()
    {
        _expView = transform.FindAssert("HUD Exp View").GetComponentAssert<HUDExpView>();
    }
    protected override void OnRelease()
    {
        _expView = default;
    }
    protected override void OnUpdatedModel()
    {
        Managers.Game.VTuber.CurExp.Subscribe(UpdateExpGauge).AddTo(CompositeDisposable);
        Managers.Game.VTuber.Level.Subscribe(UpdateLevelText).AddTo(CompositeDisposable);
    }
    private void UpdateExpGauge(int curExp)
    {
        float maxExp = Managers.Game.VTuber.MaxExp.Value;
        _expView.GaugeImage.fillAmount = curExp / maxExp;
    }
    private void UpdateLevelText(int level)
    {
        _expView.LevelText.text = level.ToString();
    }
}