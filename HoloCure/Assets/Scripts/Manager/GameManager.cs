using StringLiterals;
using System.Collections;
using UniRx;
using UniRx.Triggers;
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
    private HudPopup _hudPopup;
    public void Init()
    {
        _countTimeCo = CountTimeCo();
    }

    public void GameStart(VTuberID id, int mode, int stage)
    {
        IngameStart(stage);
               
        SelectVTuber(id);

        StartCoroutine(_countTimeCo);

        this.UpdateAsObservable()
            .Subscribe(OnPressPauseKey).AddTo(VTuber.gameObject);

        static void OnPressPauseKey(Unit unit)
        {
            if (Time.timeScale < 1) { return; }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Managers.UI.OpenPopup<PausePopup>();
            }
        }
    }

    public void GameEnd()
    {
        OutgameStart();

        StopCoroutine(_countTimeCo);
    }

    private void IngameStart(int stage)
    {
        Time.timeScale = 1.0f;
        Stage.Value = stage;
        Minutes.Value = 0;
        Seconds.Value = 0;
        Coins.Value = 0;
        DefeatedEnemies.Value = 0;

        _hudPopup = Managers.UI.OpenPopup<HudPopup>();
    }
    private void OutgameStart()
    {
        Managers.Resource.Destroy(VTuber.gameObject);
        _hudPopup.ClosePopupUI();

        Stage.Value = 0;

        Managers.UI.OpenPopup<TitlePopup>();
    }
    private void SelectVTuber(VTuberID id)
    {
        VTuberData data = Managers.Data.VTuber[id];

        VTuber = Managers.Resource.Instantiate(FileNameLiteral.VTUBER).GetComponent<VTuber>();
        VTuber.Init(id);
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
}