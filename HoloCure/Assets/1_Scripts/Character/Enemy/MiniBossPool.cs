using UnityEngine;
using Util.Pool;
public class MiniBossPool
{
    private MiniBoss _miniBossPrefab;
    private ObjectPool<MiniBoss> _miniBossPool;

    public MiniBoss GetMiniBossFromPool() => _miniBossPool.Get();

    public void Initialize(MiniBoss miniBossPrefab)
    {
        _miniBossPrefab = miniBossPrefab;
        InitializeMiniBossPool();
    }
    private void InitializeMiniBossPool() => _miniBossPool = new ObjectPool<MiniBoss>(CreateMiniBoss, OnGetMiniBossFromPool, OnReleaseMiniBossToPool, OnDestroyMiniBoss);
    private MiniBoss CreateMiniBoss()
    {
        MiniBoss miniBoss = Object.Instantiate(_miniBossPrefab);

        miniBoss.SetPoolRef(_miniBossPool);

        return miniBoss;
    }
    private void OnGetMiniBossFromPool(MiniBoss miniBoss) => miniBoss.gameObject.SetActive(true);
    private void OnReleaseMiniBossToPool(MiniBoss miniBoss) => miniBoss.gameObject.SetActive(false);
    private void OnDestroyMiniBoss(MiniBoss miniBoss) => Object.Destroy(miniBoss.gameObject);
}