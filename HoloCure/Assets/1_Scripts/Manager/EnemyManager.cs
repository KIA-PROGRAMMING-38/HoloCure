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

    private Dictionary<EnemyID, EnemyPool> _enemyPools;
    private MiniBossPool _miniBossPool;
    private Dictionary<EnemyID, IEnumerator> _spawnEnemyCoroutines;
    private Dictionary<MiniBossID, IEnumerator> _spawnMiniBossCoroutines;
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

        _enemyPools = new Dictionary<EnemyID, EnemyPool>();
        _spawnEnemyCoroutines = new Dictionary<EnemyID, IEnumerator>();
        _spawnInterval = Util.TimeStore.GetWaitForSeconds(1);

        _defaultDamageTextPool = new DamageTextPool();
        DamageText damageTextPrefab = Resources.Load<DamageText>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.DEFAULT_DAMAGE_TEXT));
        _defaultDamageTextPool.Initialize(damageTextPrefab);

        _criticalDamageTextPool = new DamageTextPool();
        damageTextPrefab = Resources.Load<DamageText>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.CRITICAL_DAMAGE_TEXT));
        _criticalDamageTextPool.Initialize(damageTextPrefab);

        _spawnMiniBossCoroutines = new Dictionary<MiniBossID, IEnumerator>();
        _miniBossPool = new MiniBossPool();
        MiniBoss miniBossDefaultPrefab = Resources.Load<MiniBoss>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.MINI_BOSS));
        _miniBossPool.Initialize(miniBossDefaultPrefab);
    }

    private bool _isSelected; // 테스트용 코드
    private void Update()
    {
        if (false == _isSelected && Input.GetKeyDown(KeyCode.P)) // 임시 코드
        {
            _isSelected = true;
            SpawnEnemy();
            SpawnMiniBoss();
        }
    }
    private void SpawnEnemy()
    {
        foreach (KeyValuePair<EnemyID, Enemy> keyValuePair in _enemyDataTable.EnemyPrefabContainer)
        {
            EnemyID enemyID = keyValuePair.Key;
            Enemy enemy = keyValuePair.Value;

            if (_enemyPools.ContainsKey(enemyID))
            {
                continue;
            }
            EnemyPool enemyPool = new EnemyPool();
            enemyPool.Initialize(enemyID, enemy, _enemyDataTable);
            _enemyPools.Add(enemyID, enemyPool);

            IEnumerator spawnEnemyCoroutine = SpawnEnemyCoroutine(enemyID);
            StartCoroutine(spawnEnemyCoroutine);
            _spawnEnemyCoroutines.Add(enemyID, spawnEnemyCoroutine);
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
    private IEnumerator SpawnEnemyCoroutine(EnemyID ID)
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
        foreach (MiniBossID ID in _enemyDataTable.StageOneMiniBossList)
        {
            IEnumerator spawnMiniBossCoroutine = SpawnMiniBossCoroutine(ID);
            StartCoroutine(spawnMiniBossCoroutine);
            _spawnMiniBossCoroutines.Add(ID, spawnMiniBossCoroutine);
        }
    }
    private IEnumerator SpawnMiniBossCoroutine(MiniBossID ID)
    {
        yield return Util.TimeStore.GetWaitForSeconds(_enemyDataTable.MiniBossFeatureContainer[ID].SpawnStartTime);

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
        miniBossInstance.InitializeStatus(_enemyDataTable.MiniBossStatContainer[ID], _enemyDataTable.MiniBossFeatureContainer[ID]);
        miniBossInstance.SetEnemyRender(_enemyDataTable.MiniBossRenderContainer[ID]);

        miniBossInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
        miniBossInstance.SetFilpX();
        miniBossInstance.SetDefaultDamageTextPool(_defaultDamageTextPool);
        miniBossInstance.SetCriticalDamageTextPool(_criticalDamageTextPool);

        miniBossInstance.OnDieForSpawnEXP -= GameManager.ObjectManager.SpawnEXP;
        miniBossInstance.OnDieForSpawnEXP += GameManager.ObjectManager.SpawnEXP;

        miniBossInstance.OnDieForUpdateCount -= GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;
        miniBossInstance.OnDieForUpdateCount += GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;

        StopCoroutine(_spawnMiniBossCoroutines[ID]);
    }
}