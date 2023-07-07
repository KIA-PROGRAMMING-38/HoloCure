using StringLiterals;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject _enemyContainer;
    private Dictionary<int, EnemyPool> _enemyPools;
    private MiniBossPool _miniBossPool;
    private Dictionary<int, BossPool> _bossPools;
    private Dictionary<int, IEnumerator> _spawnEnemyCoroutines;
    private DamageTextPool _defaultDamageTextPool;
    private DamageTextPool _criticalDamageTextPool;
    private void Start()
    {
        int widthDiv = WIDTH / WIDTH_RATE;
        int heightDiv = HEIGHT / HEIGHT_RATE;
        _widths = new int[WIDTH_RATE + 1];
        _heights = new int[HEIGHT_RATE + 1];
        for (int i = 0; i <= WIDTH_RATE; ++i)
        {
            _widths[i] = i * widthDiv - WIDTH / 2;
        }
        for (int i = 0; i <= HEIGHT_RATE; ++i)
        {
            _heights[i] = i * heightDiv - HEIGHT / 2;
        }

        _enemyPools = new Dictionary<int, EnemyPool>();
        _miniBossPool = new MiniBossPool();
        MiniBoss miniBossDefaultPrefab = Resources.Load<MiniBoss>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.MINI_BOSS));
        _miniBossPool.Initialize(miniBossDefaultPrefab);
        _bossPools = new Dictionary<int, BossPool>();

        _spawnEnemyCoroutines = new Dictionary<int, IEnumerator>();

        _spawnInterval = Util.TimeStore.GetWaitForSeconds(1);

        _defaultDamageTextPool = new();
        DamageText damageTextPrefab = Resources.Load<DamageText>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.DEFAULT_DAMAGE_TEXT));
        _defaultDamageTextPool.Initialize(damageTextPrefab);

        _criticalDamageTextPool = new();
        damageTextPrefab = Resources.Load<DamageText>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.CRITICAL_DAMAGE_TEXT));
        _criticalDamageTextPool.Initialize(damageTextPrefab);

        Managers.PresenterM.TitleUIPresenter.OnPlayGame -= StartStageOne;
        Managers.PresenterM.TitleUIPresenter.OnPlayGame += StartStageOne;

        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= GameEnd;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += GameEnd;

        _enemyContainer = new GameObject("EnemyContainer");

        SetSpawn();
    }
    
    private void SetSpawn()
    {
        SetSpawnEnemy();
        SetSpawnBoss();
    }
    private void GameEnd()
    {
        StopAllCoroutines();
        _enemyContainer.SetActive(false);
    }
    private void StartStageOne()
    {
        _enemyContainer.SetActive(true);
        _spawnEnemyCoroutines.Clear();
        foreach (int ID in Managers.DataTableM.EnemyDataTable.StageOneEnemyList)
        {
            IEnumerator spawnEnemyCoroutine = SpawnEnemyCoroutine(ID);
            StartCoroutine(spawnEnemyCoroutine);
            _spawnEnemyCoroutines.Add(ID, spawnEnemyCoroutine);
        }
        foreach (int ID in Managers.DataTableM.EnemyDataTable.StageOneMiniBossList)
        {
            IEnumerator spawnMiniBossCoroutine = SpawnMiniBossCoroutine(ID);
            StartCoroutine(spawnMiniBossCoroutine);
            _spawnEnemyCoroutines.Add(ID, spawnMiniBossCoroutine);
        }
        foreach (int ID in Managers.DataTableM.EnemyDataTable.StageOneBossList)
        {
            IEnumerator spawnBossCoroutine = SpawnBossCoroutine(ID);
            StartCoroutine(spawnBossCoroutine);
            _spawnEnemyCoroutines.Add(ID, spawnBossCoroutine);
        }
    }
    private void SetSpawnEnemy()
    {
        foreach (int ID in Managers.DataTableM.EnemyDataTable.StageOneEnemyList)
        {
            if (_enemyPools.ContainsKey(ID))
            {
                continue;
            }
            EnemyPool enemyPool = new();
            enemyPool.Initialize(ID, Managers.DataTableM.EnemyDataTable);
            _enemyPools.Add(ID, enemyPool);
        }
    }
    private int[] _widths;
    private int[] _heights;
    private const int WIDTH = 1280;
    private const int HEIGHT = 720;
    private const int WIDTH_RATE = 16;
    private const int HEIGHT_RATE = 9;
    private Vector2 _spawnPos;
    private WaitForSeconds _spawnInterval;
    private IEnumerator SpawnEnemyCoroutine(int ID)
    {
        yield return Util.TimeStore.GetWaitForSeconds(Managers.DataTableM.EnemyDataTable.EnemyFeatureContainer[ID].SpawnStartTime);

        while (Managers.StageM.CurrentStageTime < Managers.DataTableM.EnemyDataTable.EnemyFeatureContainer[ID].SpawnEndTime)
        {
            int x, y;
            if (Random.Range(0, WIDTH) < HEIGHT)
            {
                x = _widths[Random.Range(1, WIDTH_RATE)];
                y = Random.Range(0, 2) == 0 ? _heights[1] : _heights[HEIGHT_RATE - 1];
            }
            else
            {
                x = Random.Range(0, 2) == 0 ? _widths[1] : _widths[WIDTH_RATE - 1];
                y = _heights[Random.Range(1, HEIGHT_RATE)];
            }
            _spawnPos.Set(x, y);
            Enemy enemyInstance = _enemyPools[ID].GetEnemyFromPool();
            enemyInstance.transform.parent = _enemyContainer.transform;
            enemyInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
            enemyInstance.SetFilpX();
            enemyInstance.SetDefaultDamageTextPool(_defaultDamageTextPool);
            enemyInstance.SetCriticalDamageTextPool(_criticalDamageTextPool);

            enemyInstance.OnDieForSpawnEXP -= Managers.ObjectM.SpawnEXP;
            enemyInstance.OnDieForSpawnEXP += Managers.ObjectM.SpawnEXP;

            enemyInstance.OnDieForUpdateCount -= Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;
            enemyInstance.OnDieForUpdateCount += Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;

            yield return _spawnInterval;
        }

        StopCoroutine(_spawnEnemyCoroutines[ID]);
    }
    private IEnumerator SpawnMiniBossCoroutine(int ID)
    {
        yield return Util.TimeStore.GetWaitForSeconds(Managers.DataTableM.EnemyDataTable.EnemyFeatureContainer[ID].SpawnStartTime);

        int x, y;
        if (Random.Range(0, WIDTH) < HEIGHT)
        {
            x = _widths[Random.Range(1, WIDTH_RATE)];
            y = Random.Range(0, 2) == 0 ? _heights[1] : _heights[HEIGHT_RATE - 1];
        }
        else
        {
            x = Random.Range(0, 2) == 0 ? _widths[1] : _widths[WIDTH_RATE - 1];
            y = _heights[Random.Range(1, HEIGHT_RATE)];
        }
        _spawnPos.Set(x, y);
        MiniBoss miniBossInstance = _miniBossPool.GetMiniBossFromPool();
        miniBossInstance.transform.parent = _enemyContainer.transform;
        miniBossInstance.InitializeStatus(Managers.DataTableM.EnemyDataTable.EnemyStatContainer[ID], Managers.DataTableM.EnemyDataTable.EnemyFeatureContainer[ID]);
        miniBossInstance.SetEnemyRender(Managers.DataTableM.EnemyDataTable.EnemyRenderContainer[ID]);

        miniBossInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
        miniBossInstance.SetFilpX();
        miniBossInstance.SetDefaultDamageTextPool(_defaultDamageTextPool);
        miniBossInstance.SetCriticalDamageTextPool(_criticalDamageTextPool);

        miniBossInstance.OnDieForSpawnEXP -= Managers.ObjectM.SpawnEXP;
        miniBossInstance.OnDieForSpawnEXP += Managers.ObjectM.SpawnEXP;

        miniBossInstance.OnDieForSpawnBox -= Managers.ObjectM.SpawnBox;
        miniBossInstance.OnDieForSpawnBox += Managers.ObjectM.SpawnBox;

        miniBossInstance.OnDieForUpdateCount -= Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;
        miniBossInstance.OnDieForUpdateCount += Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;

        StopCoroutine(_spawnEnemyCoroutines[ID]);
    }

    private void SetSpawnBoss()
    {
        foreach (int ID in Managers.DataTableM.EnemyDataTable.StageOneBossList)
        {
            if (_bossPools.ContainsKey(ID))
            {
                continue;
            }
            BossPool bossPool = new();
            bossPool.Initialize(ID, Managers.DataTableM.EnemyDataTable);
            _bossPools.Add(ID, bossPool);
        }
    }
    private IEnumerator SpawnBossCoroutine(int ID)
    {
        yield return Util.TimeStore.GetWaitForSeconds(Managers.DataTableM.EnemyDataTable.EnemyFeatureContainer[ID].SpawnStartTime);

        int x, y;
        if (Random.Range(0, WIDTH) < HEIGHT)
        {
            x = _widths[Random.Range(1, WIDTH_RATE)];
            y = Random.Range(0, 2) == 0 ? _heights[1] : _heights[HEIGHT_RATE - 1];
        }
        else
        {
            x = Random.Range(0, 2) == 0 ? _widths[1] : _widths[WIDTH_RATE - 1];
            y = _heights[Random.Range(1, HEIGHT_RATE)];
        }
        _spawnPos.Set(x, y);
        Boss bossInstance = _bossPools[ID].GetBossFromPool();
        bossInstance.transform.parent = _enemyContainer.transform;
        bossInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
        bossInstance.SetFilpX();
        bossInstance.SetDefaultDamageTextPool(_defaultDamageTextPool);
        bossInstance.SetCriticalDamageTextPool(_criticalDamageTextPool);

        bossInstance.OnDieForSpawnEXP -= Managers.ObjectM.SpawnEXP;
        bossInstance.OnDieForSpawnEXP += Managers.ObjectM.SpawnEXP;

        bossInstance.OnDieForSpawnBox -= Managers.ObjectM.SpawnBox;
        bossInstance.OnDieForSpawnBox += Managers.ObjectM.SpawnBox;

        bossInstance.OnDieForUpdateCount -= Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;
        bossInstance.OnDieForUpdateCount += Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;

        bossInstance.OnDieForStage -= Managers.StageM.CountBoss;
        bossInstance.OnDieForStage += Managers.StageM.CountBoss;

        StopCoroutine(_spawnEnemyCoroutines[ID]);
    }
}

