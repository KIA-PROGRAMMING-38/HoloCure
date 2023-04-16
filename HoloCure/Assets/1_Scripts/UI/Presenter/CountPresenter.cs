using System;

public class CountPresenter
{
    public event Action<int> OnUpdatePlayerLevelCount;
    public event Action<int> OnUpdateCoinCount;
    public event Action<int> OnUpdateDefeatedEnemyCount;

    private int _playerLevelCount;
    private int _coinCount;
    private int _defeatedEnemyCount;

    public void UpdatePlayerLevelCount() => OnUpdatePlayerLevelCount?.Invoke(++_playerLevelCount);
    public void UpdateCoinCount() => OnUpdateCoinCount?.Invoke(++_coinCount);
    public void UpdateDefeatedEnemyCount() => OnUpdateDefeatedEnemyCount?.Invoke(++_defeatedEnemyCount);
}
