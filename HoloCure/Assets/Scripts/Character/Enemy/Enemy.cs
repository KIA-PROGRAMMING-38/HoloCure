using StringLiterals;
using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public Transform VTuberTransform => _VTuberTransform;
    private Transform _VTuberTransform;

    private Transform _body;
    private EnemyAnimation _enemyAnimation;
    private SpriteRenderer _spriteRenderer;

    private Transform _dieEffect;

    private void Awake()
    {
        _body = transform.Find(GameObjectLiteral.BODY);
        _enemyAnimation = _body.GetComponent<EnemyAnimation>();
        _spriteRenderer = _body.GetComponent<SpriteRenderer>();

        _dieEffect = transform.Find(GameObjectLiteral.DIE_EFFECT);

        GetComponent<CircleCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().freezeRotation = true;
        maxHealth = 8;
        atkPower = 2;
        moveSpeed *= 0.35f;

        
    }
    protected override void OnEnable()
    {
        Spawn();
    }
    public override void Move()
    {
        Vector2 moveVec = _VTuberTransform.position - transform.position;
        transform.Translate(moveVec.normalized * (moveSpeed * Time.deltaTime));
    }
    public override void Attack(CharacterBase target)
    {
        target.TakeDamage((int)atkPower);
    }
    private void Spawn()
    {
        moveSpeed = 80f * 0.35f;
        _dieEffect.gameObject.SetActive(false);
        _body.position = transform.position;
        _enemyAnimation.SetSpawn();
    }
    public override void Die()
    {
        Vector2 dyingPoint = transform.position;

        moveSpeed = 0f;

        _dyingMoveCoroutine = DyingMove(_body, dyingPoint, _spriteRenderer.flipX == true ? Vector2.right : Vector2.left);
        StartCoroutine(_dyingMoveCoroutine);

        _enemyAnimation.SetDie();
        _dieEffect.gameObject.SetActive(true);
    }

    // 사망시 움직임
    private float _elapsedTime;
    private IEnumerator _dyingMoveCoroutine;
    private IEnumerator DyingMove(Transform bodyTransform, Vector2 dyingPoint, Vector2 dir)
    {
        while (true)
        {
            yield return null;

            bodyTransform.position = Vector2.Lerp(dyingPoint, dyingPoint + dir * 100, _elapsedTime / 0.7f);

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > 0.7f)
            {
                _elapsedTime = 0f;
                StopCoroutine(_dyingMoveCoroutine);
                gameObject.SetActive(false);
            }
        }
    }

    // 참조
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.GRID_SENSOR))
        {
            if (_VTuberTransform != null)
            {
                return;
            }
            _VTuberTransform = collision.transform.root.GetComponent<Transform>();
            gameObject.layer = LayerNum.HAVE_REF_ENEMY;
        }
    }
}
