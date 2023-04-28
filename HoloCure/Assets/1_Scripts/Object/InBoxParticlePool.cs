using StringLiterals;
using System.Collections;
using System.IO;
using UnityEngine;
using Util.Pool;

public class InBoxParticlePool : MonoBehaviour
{
    private InBoxParticle _inBoxParticlePrefab;
    private ObjectPool<InBoxParticle> _inBoxParticlePool;
    private void Awake()
    {
        _inBoxParticlePrefab = Resources.Load<InBoxParticle>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.IN_BOX_PARTICLE));
        InitializeInBoxParticlePool();
        _spawnCoroutine = SpawnCoroutine();
    }
    private void OnEnable() => StartCoroutine(_spawnCoroutine);
    private float _elapsedTime;
    private IEnumerator _spawnCoroutine;
    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            _elapsedTime = 0;

            while (_elapsedTime < 0.75f)
            {
                _elapsedTime += Time.unscaledDeltaTime;

                _inBoxParticlePool.Get().Initialize();
                _inBoxParticlePool.Get().Initialize();
                _inBoxParticlePool.Get().Initialize();
                _inBoxParticlePool.Get().Initialize();

                yield return null;
            }

            StopCoroutine(_spawnCoroutine);

            yield return null;
        }
    }
    private void InitializeInBoxParticlePool() => _inBoxParticlePool = new ObjectPool<InBoxParticle>(CreateInBoxParticle, OnGetInBoxParticleFromPool, OnReleaseInBoxParticleToPool, OnDestroyInBoxParticle);
    private InBoxParticle CreateInBoxParticle()
    {
        InBoxParticle inBoxParticle = Instantiate(_inBoxParticlePrefab);
        inBoxParticle.transform.SetParent(transform);
        inBoxParticle.SetPoolRef(_inBoxParticlePool);

        return inBoxParticle;
    }
    private void OnGetInBoxParticleFromPool(InBoxParticle inBoxParticle) => inBoxParticle.gameObject.SetActive(true);
    private void OnReleaseInBoxParticleToPool(InBoxParticle inBoxParticle) => inBoxParticle.gameObject.SetActive(false);
    private void OnDestroyInBoxParticle(InBoxParticle inBoxParticle) => Destroy(inBoxParticle.gameObject);
}
