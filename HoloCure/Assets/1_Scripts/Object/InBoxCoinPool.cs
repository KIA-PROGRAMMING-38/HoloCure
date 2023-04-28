using StringLiterals;
using System.IO;
using UnityEngine;
using Util.Pool;

public class InBoxCoinPool : MonoBehaviour
{
    private InBoxCoin _inBoxCoinPrefab;
    private ObjectPool<InBoxCoin> _inBoxCoinPool;
    private void Awake()
    {
        _inBoxCoinPrefab = Resources.Load<InBoxCoin>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.IN_BOX_COIN));
        InitializeInBoxCoinPool();
    }
    private void OnEnable()
    {
        for (int i = 0; i < 100; ++i)
        {
            _inBoxCoinPool.Get().Initialize();
        }
    }
    private void InitializeInBoxCoinPool() => _inBoxCoinPool = new ObjectPool<InBoxCoin>(CreateInBoxCoin, OnGetInBoxCoinFromPool, OnReleaseInBoxCoinToPool, OnDestroyInBoxCoin);
    private InBoxCoin CreateInBoxCoin()
    {
        InBoxCoin inBoxCoin = Instantiate(_inBoxCoinPrefab);
        inBoxCoin.transform.SetParent(transform);
        inBoxCoin.SetPoolRef(_inBoxCoinPool);

        return inBoxCoin;
    }
    private void OnGetInBoxCoinFromPool(InBoxCoin inBoxCoin) => inBoxCoin.gameObject.SetActive(true);
    private void OnReleaseInBoxCoinToPool(InBoxCoin inBoxCoin) => inBoxCoin.gameObject.SetActive(false);
    private void OnDestroyInBoxCoin(InBoxCoin inBoxCoin) => Destroy(inBoxCoin.gameObject);
}
