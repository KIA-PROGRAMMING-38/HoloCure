using System.Collections;
using UnityEngine;

public class SmollAme : Enemy
{
    private const float JUMP_TIME = 0.5f;
    private const float CHASE_FAST_TIME = 0.5f;
    private const int CHASE_SLOW_TIME = 3;

    private static readonly Vector2 START_SHADOW_SCALE = new Vector2(26, 15.6f);
    private static readonly Vector2 END_SHADOW_SCALE = new Vector2(130, 78);

    private static readonly Vector2 BODY_ATTACK_OFFSET = new Vector2(0, 33);
    private static readonly Vector2 BODY_ATTACK_SIZE = new Vector2(45, 65);
    private Vector2 _bodyDefaultOffset;
    private Vector2 _bodyDefaultSize;

    private enum ShadowID { Default = 0, Jump = 1 }
    private GameObject[] _shadows;
    private GameObject _attackEffect;

    private SmollAmeAnimationEvent _animation;
    private BoxCollider2D _bodyCollider;
    private CapsuleCollider2D _attackCollider;

    private IEnumerator _jumpCo;
    private IEnumerator _chaseCo;

    protected override void Awake()
    {
        base.Awake();

        _shadows = new GameObject[2]
        {
            body.GetChild((int)ShadowID.Default).gameObject,
            body.GetChild((int)ShadowID.Jump).gameObject
        };
        _attackEffect = transform.FindAssert("AttackEffect").gameObject;

        _animation = body.GetComponentAssert<SmollAmeAnimationEvent>();
        _bodyCollider = gameObject.GetComponentAssert<BoxCollider2D>();
        _attackCollider = gameObject.GetComponentAssert<CapsuleCollider2D>();

        _bodyDefaultOffset = _bodyCollider.offset;
        _bodyDefaultSize = _bodyCollider.size;
    }

    protected override void Start()
    {
        base.Start();

        AddEvent();

        _jumpCo = JumpCo();
        _chaseCo = ChaseCo();
    }

    protected override void Release()
    {
        Managers.Game.GameClear();

        Managers.Resource.Destroy(gameObject);
    }

    private void HandleJumpReady() => moveSpeed = 0;

    private void HandleJumpStart()
    {
        _bodyCollider.enabled = false;

        ActivateJumpShadow();

        StartCoroutine(_jumpCo);

        Managers.Sound.Play(SoundID.SmollAmeJump);
    }

    private void HandleAttackReady() => ActivateDefaultShadow();

    private void HandleAttackStart()
    {
        _attackEffect.SetActive(true);

        _attackCollider.enabled = true;

        Util.CMCamera.Shake();

        Managers.Sound.Play(SoundID.SmollAmeAttack);
    }

    private void HandleAttackEnd()
    {
        _attackCollider.enabled = false;
        _bodyCollider.enabled = true;
        SetBodyColliderAttack();
    }

    private void HandleAttackRelease()
    {
        _attackEffect.SetActive(false);

        SetBodyColliderDefault();

        moveSpeed = Managers.Data.Enemy[id].Speed;
    }

    private IEnumerator JumpCo()
    {
        while (true)
        {
            float elapsedTime = 0;
            Vector2 start = START_SHADOW_SCALE;
            Vector2 end = END_SHADOW_SCALE;

            while (elapsedTime < JUMP_TIME)
            {
                elapsedTime += Time.deltaTime;
                _shadows[(int)ShadowID.Jump].transform.localScale = Vector2.Lerp(start, end, elapsedTime / JUMP_TIME);

                yield return null;
            }

            StopCoroutine(_jumpCo);

            StartCoroutine(_chaseCo);

            yield return null;
        }
    }

    private IEnumerator ChaseCo()
    {
        while (true)
        {
            float elapsedTime = 0;
            Vector2 start = transform.position;
            Vector2 end = Managers.Game.VTuber.transform.position;

            while (elapsedTime < CHASE_FAST_TIME)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector2.Lerp(start, end, elapsedTime / CHASE_FAST_TIME);

                yield return null;
            }

            moveSpeed = Managers.Data.Enemy[id].Speed / CHASE_SLOW_TIME;

            yield return Util.DelayCache.GetWaitForSeconds(CHASE_SLOW_TIME);

            moveSpeed = 0;

            StopCoroutine(_chaseCo);

            yield return null;
        }
    }

    private void ActivateJumpShadow()
    {
        _shadows[(int)ShadowID.Default].SetActive(false);
        _shadows[(int)ShadowID.Jump].SetActive(true);
        _shadows[(int)ShadowID.Jump].transform.localScale = START_SHADOW_SCALE;
    }
    private void ActivateDefaultShadow()
    {
        _shadows[(int)ShadowID.Default].SetActive(true);
        _shadows[(int)ShadowID.Jump].SetActive(false);
    }

    private void SetBodyColliderAttack()
    {
        _bodyCollider.offset = BODY_ATTACK_OFFSET;
        _bodyCollider.size = BODY_ATTACK_SIZE;
    }
    private void SetBodyColliderDefault()
    {
        _bodyCollider.offset = _bodyDefaultOffset;
        _bodyCollider.size = _bodyDefaultSize;
    }

    private void AddEvent()
    {
        RemoveEvent();

        _animation.OnJumpReady += HandleJumpReady;
        _animation.OnJumpStart += HandleJumpStart;
        _animation.OnAttackReady += HandleAttackReady;
        _animation.OnAttackStart += HandleAttackStart;
        _animation.OnAttackEnd += HandleAttackEnd;
        _animation.OnAttackRelease += HandleAttackRelease;
    }
    private void RemoveEvent()
    {
        _animation.OnJumpReady -= HandleJumpReady;
        _animation.OnJumpStart -= HandleJumpStart;
        _animation.OnAttackReady -= HandleAttackReady;
        _animation.OnAttackStart -= HandleAttackStart;
        _animation.OnAttackEnd -= HandleAttackEnd;
        _animation.OnAttackRelease -= HandleAttackRelease;
    }
}
