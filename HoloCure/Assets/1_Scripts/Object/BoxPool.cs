using StringLiterals;
using UnityEngine;
using Util.Pool;

public class BoxPool
{
    private ObjectPool<Box> _pool;
    public Box Get() => _pool.Get();
    public void Release(Box box) => _pool.Release(box);
    public void Init() => InitPool();
    private void InitPool() => _pool = new ObjectPool<Box>(Create, OnGet, OnRelease, OnDestroy);
    private Box Create()
    {
        Transform boxContainer = Managers.Pool.BoxContainer.transform;

        return Managers.Resource
            .Instantiate(FileNameLiteral.BOX, boxContainer)
            .GetComponent<Box>();
    }
    private void OnGet(Box box) => box.gameObject.SetActive(true);
    private void OnRelease(Box box) => box.gameObject.SetActive(false);
    private void OnDestroy(Box box) => Object.Destroy(box.gameObject);
}