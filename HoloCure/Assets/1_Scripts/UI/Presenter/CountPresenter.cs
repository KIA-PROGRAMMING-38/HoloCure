using System;

namespace UI.Presenter
{
    public class CountPresenter
    {
        public event Action<int> OnUpdatePlayerLevelCount;
        public event Action<int> OnUpdateCoinCount;
        public event Action<int> OnUpdateDefeatedEnemyCount;

        private int _playerLevelCount = 1;
        private int _coinCount;
        private int _defeatedEnemyCount;

        public void UpdatePlayerLevelCount() => OnUpdatePlayerLevelCount?.Invoke(++_playerLevelCount);
        public void UpdateCoinCount() => OnUpdateCoinCount?.Invoke(++_coinCount);
        public void UpdateDefeatedEnemyCount() => OnUpdateDefeatedEnemyCount?.Invoke(++_defeatedEnemyCount);
        public void ResetCount()
        {
            _playerLevelCount = 1;
            _coinCount = 0;
            _defeatedEnemyCount = 0;
            OnUpdatePlayerLevelCount?.Invoke(_playerLevelCount);
            OnUpdateCoinCount?.Invoke(_coinCount);
            OnUpdateDefeatedEnemyCount?.Invoke(_defeatedEnemyCount);
        }
    }
}