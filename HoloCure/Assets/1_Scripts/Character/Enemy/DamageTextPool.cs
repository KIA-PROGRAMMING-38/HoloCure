using UnityEngine;
using Util.Pool;

public class DamageTextPool
{
    private GameObject _prefab;
    private ObjectPool<DamageText> _pool;

    public DamageText GetDamageTextFromPool() => _pool.Get();

    public void Init(GameObject prefab)
    {
        _prefab = prefab;

        InitPool();
    }
    private void InitPool() => _pool = new ObjectPool<DamageText>(Create, OnGet, OnRelease, OnDestroy);
    private DamageText Create()
    {
        DamageText damageText = Managers.Resource.Instantiate(_prefab).GetComponent<DamageText>();

        damageText.SetPoolRef(_pool);

        return damageText;
    }
    private void OnGet(DamageText damageText) => damageText.gameObject.SetActive(true);
    private void OnRelease(DamageText damageText) => damageText.gameObject.SetActive(false);
    private void OnDestroy(DamageText damageText) => Object.Destroy(damageText.gameObject);
}