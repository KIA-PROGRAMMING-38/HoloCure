using System.Collections;
using UnityEngine;
using Util;

public class SpawnManager : MonoBehaviour
{
    private const int WIDTH = 540;
    private const int HEIGHT = 315;
    private const int ENEMY_SPAWN_OFFSET_COUNT = 36;
    private const int STAGE_CONSTANT = 1000;
    private readonly WaitForSeconds ENEMY_SPAWN_INTERVAL = TimeStore.GetWaitForSeconds(1);

    private Vector3[] _enemySpawnOffsets;

    private void Start()
    {
        Init();

        AddEvent();
    }
    private void AddEvent()
    {
        RemoveEvent();

        Managers.PresenterM.TitleUIPresenter.OnPlayGame += OnStartStage;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += OnEndStage;
    }
    private void RemoveEvent()
    {
        Managers.PresenterM.TitleUIPresenter.OnPlayGame -= OnStartStage;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= OnEndStage;
    }
    private void OnStartStage()
    {
        SpawnStageEnemies(1); // stage 1
    }
    private void OnEndStage()
    {
        StopAllCoroutines();
    }
    private void Init()
    {
        InitOffset();
    }
    private void InitOffset()
    {
        _enemySpawnOffsets = new Vector3[ENEMY_SPAWN_OFFSET_COUNT];
        int angleDiv = 360 / ENEMY_SPAWN_OFFSET_COUNT;
        for (int i = 0; i < ENEMY_SPAWN_OFFSET_COUNT; ++i)
        {
            float angle = i * angleDiv * Mathf.Rad2Deg;

            _enemySpawnOffsets[i] = new Vector3(WIDTH * Mathf.Cos(angle), HEIGHT * Mathf.Sin(angle), 0);
        }
    }
    private void SpawnStageEnemies(int stage)
    {
        foreach (var pair in Managers.Data.Enemy)
        {
            EnemyID id = pair.Key;
            EnemyID enemyType = id - stage * STAGE_CONSTANT;

            if (enemyType > EnemyID.End) { return; }

            StartCoroutine(SpawnEnemyCo(id, stage));
        }
    }
    private IEnumerator SpawnEnemyCo(EnemyID id, int stage)
    {
        EnemyData data = Managers.Data.Enemy[id];

        yield return TimeStore.GetWaitForSeconds(data.SpawnStartTime);

        EnemyID enemyType = id - stage * STAGE_CONSTANT;

        switch (enemyType)
        {
            case > EnemyID.Normal and < EnemyID.MiniBoss:
                while (Managers.StageM.CurrentStageTime < data.SpawnEndTime)
                {
                    SpawnEnemy(id);
                    yield return ENEMY_SPAWN_INTERVAL;
                }
                break;
            case > EnemyID.MiniBoss and < EnemyID.Boss:
                break;
            case > EnemyID.Boss and < EnemyID.End:
                break;
            default:
                Debug.Assert(false, $"Invalid EnemyID | ID: {id}, Stage: {stage}");
                break;
        }
    }
    private void SpawnEnemy(EnemyID id)
    {
        Enemy enemy = Managers.Pool.Enemy.Get();
        enemy.Init(id, _enemySpawnOffsets.GetRandomElement());
    }
    public void SpawnExp(Vector2 pos, int expAmount)
    {
        while (expAmount > (int)ExpAmounts.Max_Six)
        {
            Managers.Pool.Exp.Get(pos, (int)ExpAmounts.Max_Six);

            expAmount -= (int)ExpAmounts.Max_Six;
        }

        Managers.Pool.Exp.Get(pos, expAmount);
    }
    public void SpawnBox(Vector2 pos)
    {
        Box box = Managers.Pool.Box.Get();
        box.Init(pos);
    }
}