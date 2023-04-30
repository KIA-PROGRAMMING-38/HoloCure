using System;

namespace UI.Presenter
{
    public class StatPresenter
    {
        public event Action<int> OnUpdateATK;
        public event Action<int> OnUpdateSPD;
        public event Action<int> OnUpdateCRT;
        public event Action<int> OnUpdatePickup;
        public event Action<int> OnUpdateHaste;

        public void UpdateATK(int value) => OnUpdateATK?.Invoke(value);
        public void UpdateSPD(int value) => OnUpdateSPD?.Invoke(value);
        public void UpdateCRT(int value) => OnUpdateCRT?.Invoke(value);
        public void UpdatePickup(int value) => OnUpdatePickup?.Invoke(value);
        public void UpdateHaste(int value) => OnUpdateHaste?.Invoke(value);
    }
}