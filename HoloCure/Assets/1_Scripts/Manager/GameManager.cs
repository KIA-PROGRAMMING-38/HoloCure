using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnIngameStart;
    public event Action OnOutgameStart;
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
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += OutgameStart;
    }
    private void RemoveEvent()
    {
        Managers.PresenterM.TitleUIPresenter.OnPlayGameForPlayer -= SelectVTuber;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= OutgameStart;
    }
    private void IngameStart()
    {
        OnIngameStart?.Invoke();
    }
    private void OutgameStart()
    {
        DestroyGame();
        OnOutgameStart?.Invoke();

        void DestroyGame()
        {
            Managers.Resource.Destroy(Player.gameObject);
            Managers.Resource.Destroy(VTuber.gameObject);
        }
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