using StringLiterals;
using System.IO;
using UnityEngine;
using Util.Pool;

public class BoxPool
{
    private GameObject _container;
    private Box _boxPrefab;
    private ObjectPool<Box> _boxPool;

    public Box GetBoxFromPool() => _boxPool.Get();

    public void Initialize(GameObject container)
    {
        _container = container;
        _boxPrefab = Resources.Load<Box>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.BOX));

        InitializeBoxPool();
    }
    private void InitializeBoxPool() => _boxPool = new ObjectPool<Box>(CreateBox, OnGetBoxFromPool, OnReleaseBoxToPool, OnDestroyBox);
    private Box CreateBox()
    {
        Box box = Object.Instantiate(_boxPrefab, _container.transform);

        box.SetPoolRef(_boxPool);

        return box;
    }
    private void OnGetBoxFromPool(Box box) => box.gameObject.SetActive(true);
    private void OnReleaseBoxToPool(Box box) => box.gameObject.SetActive(false);
    private void OnDestroyBox(Box box) => Object.Destroy(box.gameObject);
}