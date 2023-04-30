using System;

namespace UI.Presenter
{
    public class HPPresenter
    {
        public Action<float> OnUpdateHPGauge;
        public Action<int> OnUpdateMaxHP;
        public Action<int> OnUpdateCurHP;

        private int _maxHP;

        public void UpdateCurHp(int curHP)
        {
            UnityEngine.Debug.Assert(_maxHP != 0);

            OnUpdateCurHP?.Invoke(curHP);

            OnUpdateHPGauge?.Invoke((float)curHP / _maxHP);
        }

        public void UpdateMaxHp(int value)
        {
            _maxHP = value;

            OnUpdateMaxHP?.Invoke(_maxHP);
        }
    }
}