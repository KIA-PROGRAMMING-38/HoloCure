using StringLiterals;
using UnityEngine;
using Util.Pool;

public class DamageTextPool
{
    private ObjectPool<DamageText> _pool;
    public DamageText Get() => _pool.Get();
    public void Release(DamageText damageText) => _pool.Release(damageText); 
    public void Init() => InitPool();
    private void InitPool() => _pool = new ObjectPool<DamageText>(Create, OnGet, OnRelease, OnDestroy);
    private DamageText Create()
    {
        Transform damageTextContainer = Managers.Pool.DamageTextContainer.transform;

        return Managers.Resource
            .Instantiate(FileNameLiteral.DAMAGE_TEXT, damageTextContainer)
            .GetComponent<DamageText>();
    }
    private void OnGet(DamageText damageText) => damageText.gameObject.SetActive(true);
    private void OnRelease(DamageText damageText) => damageText.gameObject.SetActive(false);
    private void OnDestroy(DamageText damageText) => Object.Destroy(damageText.gameObject);
}