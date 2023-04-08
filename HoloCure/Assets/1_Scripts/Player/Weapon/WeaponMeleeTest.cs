using StringLiterals;
using System.Collections;
using UnityEngine;

public class WeaponMeleeTest : Weapon
{
    Vector3 _localInitPos;
    protected override void Awake()
    {
        base.Awake();
        _localInitPos = transform.localPosition;
    }
    private void Update()
    {

        Move();
    }
    protected override void Move()
    {
        transform.RotateAround(transform.root.position + _localInitPos, Vector3.forward, 5);
        transform.rotation = default;
    }

    protected override void SetDamage(Enemy enemy)
    {
        enemy.GetDamage(10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.ENEMY))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SetDamage(enemy);
        }
    }

    protected override IEnumerator ActivateAttackSequence()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }
}