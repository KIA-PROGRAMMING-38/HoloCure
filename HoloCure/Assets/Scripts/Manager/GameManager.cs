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
    public int CurrentStageTime { get => Minutes.Value * 60 + Seconds.Value; }
    private HudPopup _hudPopup;
    public void Init()
    {
        _countTimeCo = CountTimeCo();
    }

    public void InGameStart(VTuberID id, int mode, int stage)
    {
        InitIngame(stage);
               
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

    public void OutgameStart()
    {
        InitOutgame();

        StopCoroutine(_countTimeCo);
    }

    private void InitIngame(int stage)
    {
        Time.timeScale = 1.0f;
        Stage.Value = stage;
        Minutes.Value = 0;
        Seconds.Value = 0;
        Coins.Value = 0;
        DefeatedEnemies.Value = 0;

        _hudPopup = Managers.UI.OpenPopup<HudPopup>();

        SoundID stageBGM = SoundID.StageOneBGM + (stage - 1);
        Managers.Sound.StopAll();
        Managers.Sound.Play(stageBGM);
    }
    private void InitOutgame()
    {
        Managers.Resource.Destroy(VTuber.gameObject);
        _hudPopup.ClosePopupUI();

        Stage.Value = 0;

        Managers.UI.OpenPopup<TitlePopup>();

        Managers.Sound.StopAll();
        Managers.Sound.Play(SoundID.TitleBGM);
    }
    private void SelectVTuber(VTuberID id)
    {
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