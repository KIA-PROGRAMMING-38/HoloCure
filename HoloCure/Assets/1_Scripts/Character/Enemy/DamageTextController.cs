using StringLiterals;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private Enemy _enemy;
    private void Awake() => _enemy = transform.root.GetComponent<Enemy>();
    private void Start()
    {
        _enemy.OnGetDamage -= SetDefaultDamageText;
        _enemy.OnGetDamage += SetDefaultDamageText;

        _enemy.OnGetCriticalDamage -= SetCriticalDamageText;
        _enemy.OnGetCriticalDamage += SetCriticalDamageText;
    }

    private DamageTextPool _defaultPool;
    /// <summary>
    /// �⺻ ������ �ؽ�ƮǮ�� ������ �����մϴ�.
    /// </summary>
    public void SetDefaultDamageTextPool(DamageTextPool pool) => _defaultPool = pool;
    private void SetDefaultDamageText(Vector2 dir,int damage)
    {
        DamageText damageText = _defaultPool.GetDamageTextFromPool();
        damageText.transform.SetParent(transform, false);
        damageText.Initialize(dir);
        damageText.GetComponent<TextMeshProUGUI>().text = NumLiteral.GetNumString(damage);
    }
    
    private DamageTextPool _criticalPool;
    /// <summary>
    /// ũ��Ƽ�� ������ �ؽ�ƮǮ�� ������ �����մϴ�.
    /// </summary>
    public void SetCriticalDamageTextPool(DamageTextPool pool) => _criticalPool = pool;
    private void SetCriticalDamageText(Vector2 dir, int damage)
    {
        DamageText damageText = _criticalPool.GetDamageTextFromPool();
        damageText.transform.SetParent(transform, false);
        damageText.Initialize(dir);
        damageText.GetComponent<TextMeshProUGUI>().text = NumLiteral.GetNumString(damage);
    }
}
