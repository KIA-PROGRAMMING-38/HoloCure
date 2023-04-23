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
        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            VTuber VTuber = collision.GetComponent<VTuber>();
            _attackCoroutine = AttackCoroutine(VTuber);
            StartCoroutine(_attackCoroutine);
        }
    }
    // 공격 종료
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            StopCoroutine(_attackCoroutine);
        }
    }

    // 공격
    private IEnumerator _attackCoroutine;
    private IEnumerator AttackCoroutine(VTuber VTuber)
    {
        while (true)
        {
            _enemy.SetDamage(VTuber);

            yield return Util.TimeStore.GetWaitForSeconds(0.2f);
        }
    }
}
