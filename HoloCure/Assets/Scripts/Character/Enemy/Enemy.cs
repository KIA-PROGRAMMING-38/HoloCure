using StringLiterals;
using UnityEngine;

public class Enemy : Character
{
    public Transform Target => _target;
    private Transform _target;
    private void Start()
    {
        moveSpeed = 50;
    }
    public override void Move()
    {
        Vector2 moveVec = _target.position - transform.position;
        transform.Translate(moveVec.normalized * (moveSpeed * Time.deltaTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.GRID_SENSOR))
        {
            if (_target != null)
            {
                return;
            }
            _target = collision.transform.root.GetComponent<Transform>();
        }
    }
}
