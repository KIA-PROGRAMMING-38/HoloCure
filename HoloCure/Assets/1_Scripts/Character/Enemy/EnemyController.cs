using StringLiterals;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Enemy _enemy;
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        _enemy.Move();
    }

    // 공격 시작
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagLiteral.VTUBER))
        {
            VTuber VTuber = collision.gameObject.GetComponent<VTuber>();
            _attackHandlerCoroutine = AttackHandler(VTuber);
            StartCoroutine(_attackHandlerCoroutine);
        }
    }
    // 공격 종료
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagLiteral.VTUBER))
        {
            StopCoroutine(_attackHandlerCoroutine);
        }
    }

    // 공격
    private WaitForSeconds _attackCoolTime = new WaitForSeconds(0.2f);
    private IEnumerator _attackHandlerCoroutine;
    private IEnumerator AttackHandler(VTuber VTuber)
    {
        while (true)
        {
            yield return _attackCoolTime;

            _enemy.SetDamage(VTuber);
        }
    }
}
