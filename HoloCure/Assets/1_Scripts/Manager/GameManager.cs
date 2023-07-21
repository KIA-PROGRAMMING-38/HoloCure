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
    private void IngameStart()
    {
        // Managers.Resouce.Instantiate("IngameEnvironment");

        Managers.UI.OpenPopup<HudPopup>();
    }
    private void OutgameStart()
    {
        Managers.Resource.Destroy(VTuber.gameObject);

        Managers.UI.OpenPopup<TitlePopup>();
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
    private void OnDestroy()
    {
        RemoveEvent();
    }
}