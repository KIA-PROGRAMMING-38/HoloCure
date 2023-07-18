using UniRx;

public class HUDHealthPresenter : Presenter
{
    private HUDHealthView _healthView;
    protected override void InitView()
    {
        _healthView = transform.FindAssert("HUD Health View").GetComponentAssert<HUDHealthView>();
    }
    protected override void OnRelease()
    {
        _healthView = default;
    }
    protected override void OnUpdatedModel()
    {
        Managers.Game.VTuber.CurHealth.Subscribe(UpdateGauge).AddTo(CompositeDisposable);
        Managers.Game.VTuber.CurHealth.Subscribe(UpdateCurHPText).AddTo(CompositeDisposable);
        Managers.Game.VTuber.MaxHealth.Subscribe(UPdateMaxHPText).AddTo(CompositeDisposable);
    }
    private void UpdateGauge(int CurHP)
    {
        float MaxHP = Managers.Game.VTuber.MaxHealth.Value;
        _healthView.GaugeImage.fillAmount = CurHP / MaxHP;
    }
    private void UpdateCurHPText(int CurHP)
    {
        _healthView.CurHPText.text = CurHP.ToString();
    }
    private void UPdateMaxHPText(int MaxHP)
    {
        _healthView.MaxHPText.text = MaxHP.ToString();
    }
}