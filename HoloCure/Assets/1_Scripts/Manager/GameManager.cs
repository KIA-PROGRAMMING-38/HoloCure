using StringLiterals;

public class GameManager
{
    public Player Player { get; private set; }
    public VTuber VTuber { get; private set; }
    public void Init()
    {
        AddEvent();
    }
    private void AddEvent()
    {
        RemoveEvent();

        Managers.PresenterM.TitleUIPresenter.OnPlayGameForPlayer += SelectVTuber;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += GameEnd;
    }
    private void RemoveEvent()
    {
        Managers.PresenterM.TitleUIPresenter.OnPlayGameForPlayer -= SelectVTuber;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= GameEnd;
    }
    private void GameEnd()
    {
        Managers.Resource.Destroy(Player.gameObject);
        Managers.Resource.Destroy(VTuber.gameObject);
    }
    private void SelectVTuber(VTuberID id)
    {
        VTuberData data = Managers.Data.VTuber[id];

        VTuber = Managers.Resource.Instantiate(FileNameLiteral.VTUBER).GetComponent<VTuber>();
        VTuber.Init(id);

        Player = VTuber.GetComponent<Player>();

        Managers.PresenterM.InitPresenter.GetInitData(data);
        Managers.PresenterM.InventoryPresenter.ResetInventory();
        Managers.PresenterM.CountPresenter.ResetCount();
    }
}