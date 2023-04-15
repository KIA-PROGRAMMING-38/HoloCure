using System;

public class PlayerLevelPresenter
{
    public event Action<int> OnUpdatePlayerLevel;
    public void UpdatePlayerLevel(int nextLevel) => OnUpdatePlayerLevel?.Invoke(nextLevel);
}
