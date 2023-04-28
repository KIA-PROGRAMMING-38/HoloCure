using StringLiterals;
using System.IO;
using UnityEngine;
using Util.Pool;

public class ParticlePool : MonoBehaviour
{
    private Particle _particlePrefab;
    private ObjectPool<Particle> _particlePool;
    private void Awake()
    {
        _particlePrefab = Resources.Load<Particle>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.PARTICLE));
        InitializeParticlePool();
    }
    private void Update()
    {
        _particlePool.Get().Initialize();
    }
    private void InitializeParticlePool() => _particlePool = new ObjectPool<Particle>(CreateParticle, OnGetParticleFromPool, OnReleaseParticleToPool, OnDestroyParticle);
    private Particle CreateParticle()
    {
        Particle particle = Instantiate(_particlePrefab);
        particle.transform.SetParent(transform);
        particle.SetPoolRef(_particlePool);

        return particle;
    }
    private void OnGetParticleFromPool(Particle particle) => particle.gameObject.SetActive(true);
    private void OnReleaseParticleToPool(Particle particle) => particle.gameObject.SetActive(false);
    private void OnDestroyParticle(Particle particle) => Destroy(particle.gameObject);
}
