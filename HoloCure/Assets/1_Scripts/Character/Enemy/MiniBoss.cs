using Util.Pool;

public class MiniBoss : Enemy
{
    protected override void ReleaseToPool() => _pool.Release(this);
    private ObjectPool<MiniBoss> _pool;
    /// <summary>
    /// 반환되어야할 풀의 주소를 설정합니다.
    /// </summary>
    public void SetPoolRef(ObjectPool<MiniBoss> pool) => _pool = pool;
}