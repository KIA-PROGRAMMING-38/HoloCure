using Cysharp.Text;
using StringLiterals;
using System.Collections;
using UnityEngine;
using Util;

public class SpawnManager : MonoBehaviour
{
    private const int WIDTH = 460;
    private const int HEIGHT = 270;
    private const int ENEMY_SPAWN_OFFSET_COUNT = 36;
    private const int STAGE_BIT = 1000;
    private const string ENEMY_CONTAINER = "Enemy Container";
    private Vector3[] _enemySpawnOffsets;
    private readonly WaitForSeconds ENEMY_SPAWN_INTERVAL = TimeStore.GetWaitForSeconds(1);

    private GameObject _enemyContainer;

    private EnemyPool _enemyPool;
    private DamageTextPool _defaultDamageTextPool;
    private DamageTextPool _criticalDamageTextPool;

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
        SpawnEnemy(1); // stage 1
    }
    private void OnEndStage()
    {
        StopAllCoroutines();
    }
    private void Init()
    {
        InitOffset();
        InitPools();
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
    private void InitPools()
    {
        _enemyContainer = new(ENEMY_CONTAINER);

        _enemyPool = new();
        _enemyPool.Init(_enemyContainer);

        _defaultDamageTextPool = new();
        _defaultDamageTextPool.Init(Managers.Resource.Load(Managers.Resource.Prefabs, ZString.Concat(PathLiteral.PREFAB, FileNameLiteral.DEFAULT_DAMAGE_TEXT)));

        _criticalDamageTextPool = new();
        _criticalDamageTextPool.Init(Managers.Resource.Load(Managers.Resource.Prefabs, ZString.Concat(PathLiteral.PREFAB, FileNameLiteral.CRITICAL_DAMAGE_TEXT)));
    }
    private void SpawnEnemy(int stage)
    {
        foreach (var pair in Managers.Data.Enemy)
        {
            EnemyID id = pair.Key;

            if (id - stage * STAGE_BIT > EnemyID.End) { return; }

            StartCoroutine(SpawnEnemyCo(id, stage));
        }
    }
    private IEnumerator SpawnEnemyCo(EnemyID id, int stage)
    {
        EnemyData data = Managers.Data.Enemy[id];

        yield return TimeStore.GetWaitForSeconds(data.SpawnStartTime);

        switch (id - stage * STAGE_BIT)
        {
            case < EnemyID.MiniBoss: // Normal
                while (Managers.StageM.CurrentStageTime < data.SpawnEndTime)
                {
                    Enemy enemy = _enemyPool.GetEnemyFromPool();
                    enemy.Init(id, _enemySpawnOffsets[Random.Range(0, ENEMY_SPAWN_OFFSET_COUNT)]);
                    enemy.SetFloatingDamagePool(_defaultDamageTextPool, _criticalDamageTextPool);

                    yield return ENEMY_SPAWN_INTERVAL;
                }
                break;
            case < EnemyID.Boss: // MiniBoss
                break;
            case < EnemyID.End: // Boss
                break;
        }
    }

}