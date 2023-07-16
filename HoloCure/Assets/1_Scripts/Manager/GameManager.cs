using StringLiterals;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ReactiveProperty<int> Stage { get; private set; } = new();
    public VTuber VTuber { get; private set; }

    public void Init()
    {
        AddEvent();
    }
    private void AddEvent()
    {
        RemoveEvent();

        Managers.PresenterM.TitleUIPresenter.OnPlayGameForPlayer += SelectVTuber;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += OutgameStart;
    }
    private void RemoveEvent()
    {
        Managers.PresenterM.TitleUIPresenter.OnPlayGameForPlayer -= SelectVTuber;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= OutgameStart;
    }
    private void IngameStart()
    {
        
    }
    private void OutgameStart()
    {
        DestroyGame();

        PopUpBGUI();

        void DestroyGame()
        {
            Managers.Resource.Destroy(VTuber.gameObject);
        }
        void PopUpBGUI()
        {
            Managers.Resource.Instantiate(FileNameLiteral.BG_UI, Managers.Spawn.OutgameContainer.transform);
        }
    }
    private void SelectVTuber(VTuberID id)
    {
        VTuberData data = Managers.Data.VTuber[id];

        VTuber = Managers.Resource.Instantiate(FileNameLiteral.VTUBER).GetComponent<VTuber>();
        VTuber.Init(id);

        Managers.PresenterM.InitPresenter.GetInitData(data);
        Managers.PresenterM.InventoryPresenter.ResetInventory();
        Managers.PresenterM.CountPresenter.ResetCount();
    }
}