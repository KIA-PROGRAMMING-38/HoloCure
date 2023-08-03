using System.Collections;
using UniRx;
using UnityEngine;
using Util;

public abstract class Weapon : MonoBehaviour
{
    public ReactiveProperty<int> Level { get; private set; } = new();
    public ItemID Id { get; private set; }

    protected Vector2 weapon2DPosition => transform.position + Vector3.left * transform.localPosition.x;
    protected WeaponLevelData weaponData;
    protected Collider2D weaponCollider;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private float _curAttackSequenceTime;
    private IEnumerator _attackCo;
    private IEnumerator _attackSequenceCo;

    public void Init(ItemID id)
    {
        InitComponents();

        Id = id;
        
        LevelUp();

        Managers.Game.VTuber.Haste
            .Subscribe(SetHaste).AddTo(this);
    }

    private void InitComponents()
    {
        _spriteRenderer = gameObject.GetComponentAssert<SpriteRenderer>();
        weaponCollider = gameObject.GetComponentAssert<Collider2D>();
        _rigidbody = gameObject.GetComponentAssert<Rigidbody2D>();
        _animator = gameObject.GetComponentAssert<Animator>();

        _spriteRenderer.enabled = false;
        weaponCollider.enabled = false;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _animator.enabled = false;
    }

    public virtual void LevelUp()
    {
        Level.Value += 1;

        weaponData = Managers.Data.WeaponLevelTable[Id][Level.Value];

        SetHaste(Managers.Game.VTuber.Haste.Value);
    }

    private void SetHaste(int haste)
    {
        float hasteRate = 1 + haste * 0.01f;
        SetAttackSequenceTime(hasteRate);
    }

    private void SetAttackSequenceTime(float hasteRate)
    {
        _curAttackSequenceTime = weaponData.BaseAttackSequenceTime / hasteRate;
        if (_curAttackSequenceTime < weaponData.MinAttackSequenceTime)
        {
            _curAttackSequenceTime = weaponData.MinAttackSequenceTime;
        }
    }

    private void Start()
    {
        _attackCo = AttackCo();
        _attackSequenceCo = AttackSequenceCo();

        StartCoroutine(_attackSequenceCo);
    }

    private IEnumerator AttackSequenceCo()
    {
        while (true)
        {
            StartCoroutine(_attackCo);

            yield return DelayCache.GetWaitForSeconds(_curAttackSequenceTime);
        }
    }

    private IEnumerator AttackCo()
    {
        while (true)
        {
            int count = 0;
            while (count < weaponData.ProjectileCount)
            {
                ShootProjectile(count);
                count += 1;

                yield return DelayCache.GetWaitForSeconds(weaponData.AttackDelay);
            }

            StopCoroutine(_attackCo);

            yield return null;
        }
    }

    protected abstract void ShootProjectile(int projectileIndex);
}
