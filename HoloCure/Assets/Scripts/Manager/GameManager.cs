using StringLiterals;
using System.Collections;
using UniRx;
using UnityEngine;
using Util;

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

        _countTimeCo = CountTimeCo();
    }

    public void GameStart(VTuberID id, int mode, int stage)
    {
        IngameStart();

        Stage.Value = stage;
        SelectVTuber(id);

        StartCoroutine(_countTimeCo);
    }

    private void IngameStart()
    {
        // Managers.Resouce.Instantiate("IngameEnvironment");

        Managers.UI.OpenPopup<HudPopup>();

        Time.timeScale = 1.0f;
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
    private IEnumerator _countTimeCo;
    private IEnumerator CountTimeCo()
    {
        while (true)
        {
            yield return DelayCache.GetWaitForSeconds(1);

            Seconds.Value += 1;

            if (Seconds.Value >= 60)
            {
                Seconds.Value = 0;
                Minutes.Value += 1;
            }
        }
    }

    public void CountDefeatedEnemies()
    {
        DefeatedEnemies.Value += 1;
    }

    private void AddEvent()
    {
        RemoveEvent();

        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += OutgameStart;
    }
    private void RemoveEvent()
    {
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= OutgameStart;
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }
}