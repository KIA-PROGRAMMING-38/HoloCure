using StringLiterals;
using UnityEngine;
using Util.Pool;

public class BoxPool
{
    private GameObject _container;
    private ObjectPool<Box> _pool;

    public Box GetBoxFromPool(Vector2 pos)
    {
        Box box = _pool.Get();

        box.Init(pos);

        return box;
    }

    public void Init(GameObject container)
    {
        _container = container;

        InitPool();
    }
    private void InitPool() => _pool = new ObjectPool<Box>(CreateBox, OnGet, OnRelease, OnDestroy);
    private Box CreateBox()
    {
        Box box = Managers.Resource.Instantiate(FileNameLiteral.BOX, _container.transform).GetComponent<Box>();

        box.SetPoolRef(_pool);

        return box;
    }
    private void OnGet(Box box) => box.gameObject.SetActive(true);
    private void OnRelease(Box box) => box.gameObject.SetActive(false);
    private void OnDestroy(Box box) => Object.Destroy(box.gameObject);
}