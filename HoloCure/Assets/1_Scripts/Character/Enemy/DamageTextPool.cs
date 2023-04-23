using UnityEngine;
using Util.Pool;

public class DamageTextPool
{
    private DamageText _damageTextPrefab;
    private ObjectPool<DamageText> _damageTextPool;

    public DamageText GetDamageTextFromPool() => _damageTextPool.Get();

    public void Initialize(DamageText damageTextPrefab)
    {
        _damageTextPrefab = damageTextPrefab;
        InitializeDamageTextPool();
    }
    private void InitializeDamageTextPool() => _damageTextPool = new ObjectPool<DamageText>(CreateDamageText, OnGetDamageTextFromPool, OnReleaseDamageTextToPool, OnDestroyDamageText);
    private DamageText CreateDamageText()
    {
        DamageText damageText = Object.Instantiate(_damageTextPrefab);
        damageText.SetPoolRef(_damageTextPool);

        return damageText;
    }
    private void OnGetDamageTextFromPool(DamageText damageText) => damageText.gameObject.SetActive(true);
    private void OnReleaseDamageTextToPool(DamageText damageText) => damageText.gameObject.SetActive(false);
    private void OnDestroyDamageText(DamageText damageText) => Object.Destroy(damageText.gameObject);
}