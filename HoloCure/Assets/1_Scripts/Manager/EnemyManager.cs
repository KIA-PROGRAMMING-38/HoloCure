using System.Collections;
using System.Collections.Generic;
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
    private Dictionary<EnemyID, IEnumerator> _spawnEnemyCoroutines;
    private void Start()
    {
        _enemyPools = new Dictionary<EnemyID, EnemyPool>();
        _spawnEnemyCoroutines = new Dictionary<EnemyID, IEnumerator>();
        _spawnInterval = Util.TimeStore.GetWaitForSeconds(1);
    }

    private bool _isSelected; // 테스트용 코드
    private void Update()
    {
        if (false == _isSelected && Input.GetKeyDown(KeyCode.P)) // 임시 코드
        {
            _isSelected = true;
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

                IEnumerator spawnEnemyCoroutine = SpawnEnemy(enemyID, enemy);
                StartCoroutine(spawnEnemyCoroutine);
                _spawnEnemyCoroutines.Add(enemyID, spawnEnemyCoroutine);
            }
        }
    }
    const int ReverseWidth = -480;
    const int Width = 480;
    const int ReverseHeight = -270;
    const int Height = 270;
    private Vector2 _spawnPos;
    private WaitForSeconds _spawnInterval;
    private IEnumerator SpawnEnemy(EnemyID ID, Enemy enemy)
    {
        yield return Util.TimeStore.GetWaitForSeconds(enemy.SpawnStartTime);

        while (GameManager.StageManager.CurrentStageTime < enemy.SpawnEndTime)
        {
            int x, y;
            if (Random.Range(0, Width) > Height)
            {
                x = Random.Range(ReverseWidth, Width);
                y = Random.Range(0, 2) == 0 ? Height : ReverseHeight;
            }
            else
            {
                x = Random.Range(0, 2) == 0 ? Width : ReverseWidth;
                y = Random.Range(ReverseHeight, Height);
            }
            _spawnPos.Set(x, y);
            Enemy enemyInstance = _enemyPools[ID].GetEnemyFromPool();
            enemyInstance.transform.position = Util.Caching.CenterWorldPos + _spawnPos;
            enemyInstance.SetFilpX();

            enemyInstance.OnDieForSpawnEXP -= GameManager.ObjectManager.SpawnEXP;
            enemyInstance.OnDieForSpawnEXP += GameManager.ObjectManager.SpawnEXP;

            enemyInstance.OnDieForUpdateCount -= GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;
            enemyInstance.OnDieForUpdateCount += GameManager.PresenterManager.CountPresenter.UpdateDefeatedEnemyCount;

            yield return _spawnInterval;
        }

        StopCoroutine(_spawnEnemyCoroutines[ID]);
    }
}