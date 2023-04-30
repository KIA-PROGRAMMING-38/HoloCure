using System;

namespace UI.Presenter
{
    public class TitleUIPresenter
    {
        public event Action OnActivateTitleBackGroundUI;
        public event Action OnDeactivateTitleBackGroundUI;

        public event Action OnActivateMainTitleUI;
        public event Action OnDeActivateMainTitleUI;

        public event Action OnActivateSelectUI;
        public event Action OnDeActivateSelectUI;

        public event Action OnPlayGame;
        public event Action<VTuberID> OnPlayGameForPlayer;
        public event Action OnPlayGameForStage;

        private void ActivateTitleBackGroundUI()
        {
            OnActivateTitleBackGroundUI?.Invoke();
        }
        public void ResetTitleBackGroundUI()
        {
            OnDeactivateTitleBackGroundUI?.Invoke();
            ActivateTitleBackGroundUI();
        }

        public void ActivateMainTitleUI()
        {
            ActivateTitleBackGroundUI();
            OnActivateMainTitleUI?.Invoke();
        }
        public void ActivateSelectUI()
        {
            OnDeActivateMainTitleUI?.Invoke();
            OnActivateSelectUI?.Invoke();
        }
        public void DeActivateSelectUI()
        {
            ResetTitleBackGroundUI();
            OnDeActivateSelectUI?.Invoke();
            OnActivateMainTitleUI?.Invoke();
        }
        public void PlayGame(VTuberID VTuberID, ModeID modeID, StageID stageID)
        {
            OnDeactivateTitleBackGroundUI?.Invoke();
            OnDeActivateSelectUI?.Invoke();
            OnPlayGameForPlayer?.Invoke(VTuberID);
            OnPlayGameForStage?.Invoke();

            OnPlayGame?.Invoke();
        }
    }
}