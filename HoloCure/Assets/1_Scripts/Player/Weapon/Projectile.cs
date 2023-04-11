using StringLiterals;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _hitLimit;
    private int _curHit;
    private Collider2D _collider;

    /// <summary>
    /// 투사체를 초기화합니다.
    /// </summary>
    /// <param name="collider">콜라이더</param>
    /// <param name="hitLimit">적중제한횟수</param>
    public void Initialize(Collider2D collider, int hitLimit)
    {
        _collider = collider;
        _hitLimit = hitLimit;
    }

    /// <summary>
    /// 충돌을 시작하기 위한 함수입니다, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void OnCollider() => _collider.enabled = true;

    /// <summary>
    /// 충돌을 다시하기 위한 함수입니다, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void ResetCollider()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }

    /// <summary>
    /// 충돌을 종료하기 위한 함수입니다, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void OffCollider() => _collider.enabled = false;


    private void OnEnable() => _curHit = _hitLimit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.ENEMY))
        {
            _curHit -= 1;
            if (_curHit == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}