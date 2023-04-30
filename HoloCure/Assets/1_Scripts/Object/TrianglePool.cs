using StringLiterals;
using System.Collections;
using System.IO;
using UnityEngine;
using Util.Pool;

public class TrianglePool : MonoBehaviour
{
    private Triangle _trianglePrefab;
    private ObjectPool<Triangle> _trianglePool;
    private void Awake()
    {
        _trianglePrefab = Resources.Load<Triangle>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.TRIANGLE));
        InitializeTrianglePool();
        _spawnCoroutine = SpawnCoroutine();
    }
    private void OnEnable() => StartCoroutine(_spawnCoroutine);
    private void OnDisable() => StopCoroutine(_spawnCoroutine);
    private float _spawnInterval;
    private IEnumerator _spawnCoroutine;
    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            _spawnInterval += Time.unscaledDeltaTime;

            if (_spawnInterval > 0.5f)
            {
                _spawnInterval = 0;
                _trianglePool.Get().Initialize();
            }

            yield return null;
        }
    }
    private void InitializeTrianglePool() => _trianglePool = new ObjectPool<Triangle>(CreateTriangle, OnGetTriangleFromPool, OnReleaseTriangleToPool, OnDestroyTriangle);
    private Triangle CreateTriangle()
    {
        Triangle triangle = Instantiate(_trianglePrefab);
        triangle.transform.SetParent(transform);
        triangle.SetPoolRef(_trianglePool);

        return triangle;
    }
    private void OnGetTriangleFromPool(Triangle triangle) => triangle.gameObject.SetActive(true);
    private void OnReleaseTriangleToPool(Triangle triangle) => triangle.gameObject.SetActive(false);
    private void OnDestroyTriangle(Triangle triangle) => Destroy(triangle.gameObject);
}
