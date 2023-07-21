using System;
using UnityEngine;

public class SelectUI : UIBaseLegacy
{
    public event Action<VTuberID,ModeID,StageID> OnPlayGame;

    [SerializeField] private SelectIconController _iconController;
    [SerializeField] private SelectModeController _modeController;
    [SerializeField] private SelectStageController _stageController;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TitleUIPresenter.OnActivateSelectUI -= ActivateSelectVTuberUI;
        PresenterManager.TitleUIPresenter.OnActivateSelectUI += ActivateSelectVTuberUI;
        PresenterManager.TitleUIPresenter.OnActivateSelectUI -= ActivateIconSelect;
        PresenterManager.TitleUIPresenter.OnActivateSelectUI += ActivateIconSelect;

        PresenterManager.TitleUIPresenter.OnDeActivateSelectUI -= DeActivateSelectVTuberUI;
        PresenterManager.TitleUIPresenter.OnDeActivateSelectUI += DeActivateSelectVTuberUI;

        _iconController.OnSelectVTuber -= ActivateModeSelect;
        _iconController.OnSelectVTuber += ActivateModeSelect;

        _modeController.OnCancel -= ActivateIconSelect;
        _modeController.OnCancel += ActivateIconSelect;

        _modeController.OnSelectStageMode -= ActivateStageSelect;
        _modeController.OnSelectStageMode += ActivateStageSelect;

        _stageController.OnCancel -= ActivateModeSelect;
        _stageController.OnCancel += ActivateModeSelect;

        _iconController.OnSelectVTuberToUI -= GetVTuberID;
        _iconController.OnSelectVTuberToUI += GetVTuberID;
        _modeController.OnSelectStageModeToUI -= GetModeID;
        _modeController.OnSelectStageModeToUI += GetModeID;
        _stageController.OnPlayToUI -= GetStageID;
        _stageController.OnPlayToUI += GetStageID;

        _stageController.OnPlay -= PlayGame;
        _stageController.OnPlay += PlayGame;

        OnPlayGame -= PresenterManager.TitleUIPresenter.PlayGame;
        OnPlayGame += PresenterManager.TitleUIPresenter.PlayGame;
    }
    private void ActivateSelectVTuberUI() => _canvas.enabled = true;
    private void DeActivateSelectVTuberUI() => _canvas.enabled = false;
    private void ActivateIconSelect()
    {
        _iconController.gameObject.SetActive(true);
        _modeController.gameObject.SetActive(false);
        _stageController.gameObject.SetActive(false);

        _iconController.StartGetKeyCoroutine();
    }
    private void ActivateModeSelect()
    {
        _iconController.gameObject.SetActive(false);
        _modeController.gameObject.SetActive(true);
        _stageController.gameObject.SetActive(false);

        _modeController.StartGetKeyCoroutine();
    }
    private void ActivateStageSelect()
    {
        _iconController.gameObject.SetActive(false);
        _modeController.gameObject.SetActive(false);
        _stageController.gameObject.SetActive(true);

        _stageController.StartGetKeyCoroutine();
    }
    private VTuberID _VTuberID;
    private void GetVTuberID(VTuberID ID) => _VTuberID = ID;
    private ModeID _modeID;
    private void GetModeID(ModeID ID) => _modeID = ID;
    private StageID _stageID;
    private void GetStageID(StageID ID) => _stageID = ID;
    private void PlayGame()
    {
        OnPlayGame?.Invoke(_VTuberID, _modeID,_stageID);
    }
}
