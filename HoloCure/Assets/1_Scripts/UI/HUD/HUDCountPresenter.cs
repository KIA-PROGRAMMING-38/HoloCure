using UniRx;

public class HUDCountPresenter : Presenter
{
    private HUDCountView _countView;
    protected override void InitView()
    {
        _countView = transform.FindAssert("HUD Count View").GetComponentAssert<HUDCountView>();
    }
    protected override void OnRelease()
    {
        _countView = default;
    }
    protected override void OnUpdatedModel()
    {
        Managers.Game.Coins.Subscribe(UpdateCoinCountText).AddTo(CompositeDisposable);
        Managers.Game.DefeatedEnemies.Subscribe(UpdateDefeatedEnemyCountText).AddTo(CompositeDisposable);
    }
    private void UpdateCoinCountText(int coinCount)
    {
        _countView.CoinCountText.text = coinCount.ToString();
    }
    private void UpdateDefeatedEnemyCountText(int enemyCount)
    {
        _countView.DefeatedEnemyCountText.text = enemyCount.ToString();
    }
}