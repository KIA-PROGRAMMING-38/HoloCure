using System;
using UnityEngine;
using Util.Pool;

public class MiniBoss : Enemy
{
    public event Action<Vector2> OnDieForSpawnBox;
    protected override void Die()
    {
        base.Die();

        OnDieForSpawnBox?.Invoke(transform.position);
    }
    protected override void SetLayerOnSpawn() => gameObject.layer = LayerNum.MY_BOSS;
    protected override void SetLayerOnDie() => gameObject.layer = LayerNum.DEAD_MY_BOSS;
    protected override void ReleaseToPool() => _pool.Release(this);
    private ObjectPool<MiniBoss> _pool;
    /// <summary>
    /// 반환되어야할 풀의 주소를 설정합니다.
    /// </summary>
    public void SetPoolRef(ObjectPool<MiniBoss> pool) => _pool = pool;
}