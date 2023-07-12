using StringLiterals;
using UnityEngine;
using Util.Pool;

public class DamageTextPool
{
    private ObjectPool<DamageText> _pool;
    public DamageText Get() => _pool.Get();
    public void Init() => InitPool();
    private void InitPool() => _pool = new ObjectPool<DamageText>(Create, OnGet, OnRelease, OnDestroy);
    private DamageText Create()
    {
        DamageText damageText = Managers.Resource.Instantiate(FileNameLiteral.DAMAGE_TEXT, Managers.Pool.DamageTextContainer.transform).GetComponent<DamageText>();

        damageText.SetPoolRef(_pool);

        return damageText;
    }
    private void OnGet(DamageText damageText) => damageText.gameObject.SetActive(true);
    private void OnRelease(DamageText damageText) => damageText.gameObject.SetActive(false);
    private void OnDestroy(DamageText damageText) => Object.Destroy(damageText.gameObject);
}