using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Util;

public abstract class Weapon : MonoBehaviour
{
    public ReactiveProperty<int> Level { get; private set; } = new();
    public ItemID Id { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
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

        GetVTuber().Haste.Subscribe(ApplyHaste).AddTo(this);
    }

    private void InitComponents()
    {
        _spriteRenderer = gameObject.GetComponentAssert<SpriteRenderer>();
        _collider = gameObject.GetComponentAssert<Collider2D>();
        _rigidbody = gameObject.GetComponentAssert<Rigidbody2D>();
        _animator = gameObject.GetComponentAssert<Animator>();

        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _animator.enabled = false;
    }

    public virtual void LevelUp()
    {
        Level.Value += 1;

        ApplyHaste(GetVTuber().Haste.Value);
    }

    private void ApplyHaste(int haste)
    {
        float hasteRate = 1 + haste * 0.01f;
        SetAttackSequenceTime(hasteRate);
    }

    private void SetAttackSequenceTime(float hasteRate)
    {
        WeaponLevelData data = GetWeaponLevelData();
        _curAttackSequenceTime = data.BaseAttackSequenceTime / hasteRate;
        if (_curAttackSequenceTime < data.MinAttackSequenceTime)
        {
            _curAttackSequenceTime = data.MinAttackSequenceTime;
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
        WeaponLevelData data = GetWeaponLevelData();
        while (true)
        {
            int count = 0;
            while (count < data.ProjectileCount)
            {
                ShootProjectile(count);
                count += 1;

                yield return DelayCache.GetWaitForSeconds(data.AttackDelay);
            }

            StopCoroutine(_attackCo);

            yield return null;
        }
    }

    protected abstract void ShootProjectile(int projectileIndex);

    protected enum ColliderType { Circle, Box }
    protected void SetCollider(Projectile projectile, ColliderType colliderType)
    {
        switch (colliderType)
        {
            case ColliderType.Circle:
                SetCollider(projectile, _collider as CircleCollider2D);
                break;
            case ColliderType.Box:
                SetCollider(projectile, _collider as BoxCollider2D);
                break;
            default: Debug.Assert(false, $"Invalid Type: Projectile-{projectile}, type-{colliderType}"); break;
        }

        static void SetCollider<T>(Projectile projectile, T weaponCollider) where T : Collider2D
        {
            T projectileCollider = projectile.AddComponent<T>();
            projectileCollider.isTrigger = true;
            projectileCollider.offset = weaponCollider.offset;

            switch (projectileCollider)
            {
                case CircleCollider2D circle:
                    circle.radius = (weaponCollider as CircleCollider2D).radius;
                    break;
                case BoxCollider2D box:
                    box.size = (weaponCollider as BoxCollider2D).size;
                    break;
            }
        }
    }

    protected Vector2 GetWeaponPosition() => transform.position;
    protected Projectile GetProjectile() => Managers.Spawn.Projectile.Get();
    protected WeaponLevelData GetWeaponLevelData() => Managers.Data.WeaponLevelTable[Id][Level.Value];
    protected VTuber GetVTuber() => Managers.Game.VTuber;
}
