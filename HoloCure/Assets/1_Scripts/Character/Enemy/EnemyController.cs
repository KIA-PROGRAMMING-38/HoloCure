using StringLiterals;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Enemy _enemy;
    private IEnumerator _attackCo;
    private const float ATTACK_INTERVAL = 0.2f;
    private void Awake() => _enemy = gameObject.GetComponentAssert<Enemy>();
    private void Start()
    {
        _attackCo = AttackCo();

        this.FixedUpdateAsObservable()
            .Subscribe(Move);

        this.OnTriggerEnter2DAsObservable()
            .Subscribe(StartAttack);

        this.OnTriggerExit2DAsObservable()
            .Subscribe(StopAttack);
    }
    private void Move(Unit unit) => _enemy.Move();
    private void StartAttack(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            StartCoroutine(_attackCo);
        }
    }
    private void StopAttack(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            StopCoroutine(_attackCo);
        }
    }
    private IEnumerator AttackCo()
    {
        while (true)
        {
            _enemy.SetDamage(Managers.Game.VTuber);

            yield return Util.DelayCache.GetWaitForSeconds(ATTACK_INTERVAL);
        }
    }
}
