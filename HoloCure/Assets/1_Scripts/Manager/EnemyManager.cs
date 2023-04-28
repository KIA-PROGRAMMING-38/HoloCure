using StringLiterals;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameManager _gameManager;
    private DataTableManager _dataTableManager;
    private EnemyDataTable _enemyDataTable;

    public GameManager GameManager
    {
        private get => _gameManager;
        set
        {
            _gameManager = value;
            _dataTableManager = _gameManager.DataTableManager;
            _enemyDataTable = _dataTableManager.EnemyDataTable;
        }
    }

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
    }

    private bool _isSelected; // 테스트용 코드
    private void Update()
    {
        if (false == _isSelected && Input.GetKeyDown(KeyCode.P)) // 임시 코드
        {
            _isSelected = true;
            SpawnEnemy();
            SpawnMiniBoss();
            SpawnBoss();
        }
    }
    private void SpawnEnemy()
    {
        foreach (int ID in _enemyDataTable.StageOneEnemyList)
        {
            if (_enemyPools.ContainsKey(ID))
            {
                continue;
            }
            EnemyPool enemyPool = new();
            enemyPool.Initialize(ID, _enemyDataTable);
            _enemyPools.Add(ID, enemyPool);

            IEnumerator spawnEnemyCoroutine = SpawnEnemyCoroutine(ID);
            StartCoroutine(spawnEnemyCoroutine);
            _spawnEnemyCoroutines.Add(ID, spawnEnemyCoroutine);
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
        yield return Util.TimeStore.GetWaitForSeconds(_enemyDataTable.EnemyFeatureContainer[ID].SpawnStartTime);

        while (GameManager.StageManager.CurrentStageTime < _enemyDataTable.EnemyFeatureContainer[ID].SpawnEndTime)
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
            enemyInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
            enemyInstance.SetFilpX();
            enemyInstance.SetDefaultDamageTextPool(_defaultDamageTextPool);
            enemyInstance.SetCriticalDamageTextPool(_criticalDamageTextPool);

            enemyInstance.OnDieForSpawnEXP -= GameManager.ObjectManager.SpawnEXP;
            enemyInstance.OnDieForSpawnEXP += GameManager.ObjectManager.SpawnEXP;

            enemyInstance.OnDieForUpdateCount -= GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;
            enemyInstance.OnDieForUpdateCount += GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;

            yield return _spawnInterval;
        }

        StopCoroutine(_spawnEnemyCoroutines[ID]);
    }

    private void SpawnMiniBoss()
    {
        foreach (int ID in _enemyDataTable.StageOneMiniBossList)
        {
            IEnumerator spawnMiniBossCoroutine = SpawnMiniBossCoroutine(ID);
            StartCoroutine(spawnMiniBossCoroutine);
            _spawnEnemyCoroutines.Add(ID, spawnMiniBossCoroutine);
        }
    }
    private IEnumerator SpawnMiniBossCoroutine(int ID)
    {
        yield return Util.TimeStore.GetWaitForSeconds(_enemyDataTable.EnemyFeatureContainer[ID].SpawnStartTime);

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
        miniBossInstance.InitializeStatus(_enemyDataTable.EnemyStatContainer[ID], _enemyDataTable.EnemyFeatureContainer[ID]);
        miniBossInstance.SetEnemyRender(_enemyDataTable.EnemyRenderContainer[ID]);

        miniBossInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
        miniBossInstance.SetFilpX();
        miniBossInstance.SetDefaultDamageTextPool(_defaultDamageTextPool);
        miniBossInstance.SetCriticalDamageTextPool(_criticalDamageTextPool);

        miniBossInstance.OnDieForSpawnEXP -= GameManager.ObjectManager.SpawnEXP;
        miniBossInstance.OnDieForSpawnEXP += GameManager.ObjectManager.SpawnEXP;

        miniBossInstance.OnDieForSpawnBox -= GameManager.ObjectManager.SpawnBox;
        miniBossInstance.OnDieForSpawnBox += GameManager.ObjectManager.SpawnBox;

        miniBossInstance.OnDieForUpdateCount -= GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;
        miniBossInstance.OnDieForUpdateCount += GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;

        StopCoroutine(_spawnEnemyCoroutines[ID]);
    }

    private void SpawnBoss()
    {
        foreach (int ID in _enemyDataTable.StageOneBossList)
        {
            if (_bossPools.ContainsKey(ID))
            {
                continue;
            }
            BossPool bossPool = new();
            bossPool.Initialize(ID, _enemyDataTable);
            _bossPools.Add(ID, bossPool);

            IEnumerator spawnBossCoroutine = SpawnBossCoroutine(ID);
            StartCoroutine(spawnBossCoroutine);
            _spawnEnemyCoroutines.Add(ID, spawnBossCoroutine);
        }
    }
    private IEnumerator SpawnBossCoroutine(int ID)
    {
        yield return Util.TimeStore.GetWaitForSeconds(_enemyDataTable.EnemyFeatureContainer[ID].SpawnStartTime);

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
        bossInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
        bossInstance.SetFilpX();
        bossInstance.SetDefaultDamageTextPool(_defaultDamageTextPool);
        bossInstance.SetCriticalDamageTextPool(_criticalDamageTextPool);

        bossInstance.OnDieForSpawnEXP -= GameManager.ObjectManager.SpawnEXP;
        bossInstance.OnDieForSpawnEXP += GameManager.ObjectManager.SpawnEXP;

        bossInstance.OnDieForSpawnBox -= GameManager.ObjectManager.SpawnBox;
        bossInstance.OnDieForSpawnBox += GameManager.ObjectManager.SpawnBox;

        bossInstance.OnDieForUpdateCount -= GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;
        bossInstance.OnDieForUpdateCount += GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;

        StopCoroutine(_spawnEnemyCoroutines[ID]);
    }
}

