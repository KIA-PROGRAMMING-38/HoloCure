using StringLiterals;
using System.Collections;
using UnityEngine;

public class FanBeam : Weapon
{
    private PlayerInput _input;
    private BoxCollider2D _collider;

    private Transform _beam;
    private BoxCollider2D _beamCollider;
    protected override void Awake()
    {
        base.Awake();
        _input = VTuber.GetComponent<PlayerInput>();

        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
    }
    private float _elapsedTime;
    protected override void Operate()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= weaponStat.AttackDurationTime)
        {
            _beamCollider.enabled = false;
            _elapsedTime = 0f;
        }
    }
    public override IEnumerator OperateSequence()
    {
        while (true)
        {
            gameObject.SetActive(true);

            Vector2 direction = _input.MouseWorldPos - (Vector2)transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _beam.position = (Vector2)transform.position + direction.normalized * 50;
            _beam.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _beamCollider.enabled = true;

            yield return attackDurationTime;

            gameObject.SetActive(false);

            yield return attackRemainTime;
        }
    }
    public override void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(weaponData, weaponStat);

        GameObject newBeam;
        newBeam = new GameObject(nameof(FanBeam));
        newBeam.transform.parent = transform;
        newBeam.layer = LayerNum.WEAPON;

        _beamCollider = newBeam.AddComponent<BoxCollider2D>();
        _beamCollider.isTrigger = true;
        _beamCollider.offset = _collider.offset;
        _beamCollider.size = _collider.size;

        newBeam.AddComponent<SpriteRenderer>();

        AnimatorOverrideController overrideController = new AnimatorOverrideController(weaponAnimator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.PROJECTILE] = weaponData.ProjectileClip;
        overrideController[AnimClipLiteral.EFFECT] = weaponData.EffectClip;

        newBeam.AddComponent<Animator>().runtimeAnimatorController = overrideController;

        _beam = newBeam.GetComponent<Transform>();
    }
}