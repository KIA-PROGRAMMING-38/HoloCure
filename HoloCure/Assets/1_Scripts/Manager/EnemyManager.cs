using Cysharp.Text;
using StringLiterals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject _enemyContainer;
    private Dictionary<EnemyID, EnemyPool> _enemyPools;
    private Dictionary<EnemyID, IEnumerator> _spawnEnemyCoroutines;
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

        _defaultDamageTextPool = new();
        DamageText damageTextPrefab = Resources.Load<DamageText>(ZString.Concat(PathLiteral.PREFAB, FileNameLiteral.DEFAULT_DAMAGE_TEXT));
        _defaultDamageTextPool.Initialize(damageTextPrefab);

        _criticalDamageTextPool = new();
        damageTextPrefab = Resources.Load<DamageText>(ZString.Concat(PathLiteral.PREFAB, FileNameLiteral.CRITICAL_DAMAGE_TEXT));
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
        foreach (KeyValuePair<EnemyID, EnemyData> pair in Managers.Data.Enemy)
        {
            EnemyID id = pair.Key;

            IEnumerator spawnEnemyCoroutine = SpawnEnemyCoroutine(id);
            StartCoroutine(spawnEnemyCoroutine);
            _spawnEnemyCoroutines.Add(id, spawnEnemyCoroutine);
        }
    }
    private void SetSpawnEnemy()
    {
        foreach (KeyValuePair<EnemyID, EnemyData> pair in Managers.Data.Enemy)
        {
            EnemyID id = pair.Key;

            if (_enemyPools.ContainsKey(id))
            {
                continue;
            }
            EnemyPool enemyPool = new();
            enemyPool.Init();
            _enemyPools.Add(id, enemyPool);
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
    private IEnumerator SpawnEnemyCoroutine(EnemyID id)
    {
        yield return Util.TimeStore.GetWaitForSeconds(Managers.Data.Enemy[id].SpawnStartTime);

        while (Managers.StageM.CurrentStageTime < Managers.Data.Enemy[id].SpawnEndTime)
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
            Enemy enemy = _enemyPools[id].GetEnemyFromPool();
            enemy.transform.parent = _enemyContainer.transform;
            enemy.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
            enemy.SetFilpX();
            enemy.SetDefaultDamageTextPool(_defaultDamageTextPool);
            enemy.SetCriticalDamageTextPool(_criticalDamageTextPool);

            enemy.OnDieForSpawnEXP -= Managers.ObjectM.SpawnEXP;
            enemy.OnDieForSpawnEXP += Managers.ObjectM.SpawnEXP;

            enemy.OnDieForUpdateCount -= Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;
            enemy.OnDieForUpdateCount += Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;

            yield return _spawnInterval;
        }

        StopCoroutine(_spawnEnemyCoroutines[id]);
    }
}

