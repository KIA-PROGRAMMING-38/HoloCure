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
    private void Start()
    {
        _enemyPools = new Dictionary<EnemyID, EnemyPool>();
        _spawnInterval = new WaitForSeconds(1);
        foreach (KeyValuePair<EnemyID, Enemy> keyValuePair in _enemyDataTable.EnemyPrefabContainer)
        {
            if (_enemyPools.ContainsKey(keyValuePair.Key))
            {
                continue;
            }
            EnemyPool enemyPool = new EnemyPool();
            enemyPool.Initialize(keyValuePair.Value);
            _enemyPools.Add(keyValuePair.Key, enemyPool);

            IEnumerator spawnEnemyCoroutine = SpawnEnemy(keyValuePair.Key, keyValuePair.Value);
            StartCoroutine(spawnEnemyCoroutine);
        }
    }

    private WaitForSeconds _spawnInterval;
    private IEnumerator SpawnEnemy(EnemyID ID, Enemy enemy)
    {
        yield return new WaitForSeconds(enemy.SpawnStartTime);

        while (Time.time < enemy.SpawnEndTime)
        {
            Enemy enemyInstance = _enemyPools[ID].GetEnemyFromPool();
            enemyInstance.transform.position = Camera.main.transform.position + Vector3.right * 100;

            yield return _spawnInterval;
        }
    }
}