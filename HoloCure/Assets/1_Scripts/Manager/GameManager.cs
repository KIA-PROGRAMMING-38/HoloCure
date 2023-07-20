using StringLiterals;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ReactiveProperty<int> Stage { get; private set; } = new();
    public ReactiveProperty<int> Minutes { get; private set; } = new();
    public ReactiveProperty<int> Seconds { get; private set; } = new();
    public ReactiveProperty<int> Coins { get; private set; } = new();
    public ReactiveProperty<int> DefeatedEnemies { get; private set; } = new();
    public VTuber VTuber { get; private set; }

    public void Init()
    {
        AddEvent();
    }
    private void IngameStart()
    {
        // Managers.Resouce.Instantiate("IngameEnvironment");

        Managers.UI.OpenPopupUI<HUDPopup>();
    }
    private void OutgameStart()
    {
        Managers.Resource.Destroy(VTuber.gameObject);

        Managers.UI.OpenPopupUI<TitlePopup>();
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