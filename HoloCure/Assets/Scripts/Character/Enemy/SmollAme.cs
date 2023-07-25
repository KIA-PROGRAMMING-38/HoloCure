using System.Collections;
using UnityEngine;

public class SmollAme : Enemy
{
    private SmollAmeAnimation _animation;
    private BoxCollider2D _bodyCollider;
    private CapsuleCollider2D _attackCollider;

    [SerializeField] private GameObject[] Shadows;
    private enum ShadowID { Default = 0, Jump = 1 }
    private readonly Vector3 START_SHADOW_SCALE = new(26, 15.6f, 1);
    private readonly Vector3 END_SHADOW_SCALE = new(130, 78, 1);

    [SerializeField] private GameObject _attackEffect;

    private Vector2 _defaultOffset;
    private Vector2 _defaultSize;
    private readonly Vector2 ATTACK_OFFSET = new(0, 33);
    private readonly Vector2 ATTACK_SIZE = new(45, 65);

    protected override void Awake()
    {
        base.Awake();

        _animation = body.GetComponent<SmollAmeAnimation>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        _attackCollider = GetComponent<CapsuleCollider2D>();

        _defaultOffset = _bodyCollider.offset;
        _defaultSize = _bodyCollider.size;
    }
    protected override void Start()
    {
        base.Start();

        _animation.OnJumpStart -= SetMoveSpeedZero;
        _animation.OnJumpStart += SetMoveSpeedZero;

        _animation.OnJump -= DeActivateBodyCollider;
        _animation.OnJump += DeActivateBodyCollider;
        _animation.OnJump += ActivateJumpShadow;
        _animation.OnJump += ActivateJumpShadow;

        _animation.OnAttackStart -= DeActivateJumpShadow;
        _animation.OnAttackStart += DeActivateJumpShadow;
        _animation.OnAttackStart += SetMoveSpeedZero;
        _animation.OnAttackStart += SetMoveSpeedZero;

        _animation.OnAttackEffectActivate -= ActivateAttackEffect;
        _animation.OnAttackEffectActivate += ActivateAttackEffect;

        _animation.OnAttack -= ActivateAttackCollider;
        _animation.OnAttack += ActivateAttackCollider;
        _animation.OnAttack -= ShakeCamera;
        _animation.OnAttack += ShakeCamera;

        _animation.OnAttackEnd -= DeActivateAttackCollider;
        _animation.OnAttackEnd += DeActivateAttackCollider;
        _animation.OnAttackEnd -= SetBodyColliderAttack;
        _animation.OnAttackEnd += SetBodyColliderAttack;
        _animation.OnAttackEnd -= ActivateBodyCollider;
        _animation.OnAttackEnd += ActivateBodyCollider;

        _animation.OnAttackRelease -= SetBodyColliderDefault;
        _animation.OnAttackRelease += SetBodyColliderDefault;
        _animation.OnAttackRelease -= DeActivateAttackEffect;
        _animation.OnAttackRelease += DeActivateAttackEffect;
        _animation.OnAttackRelease -= SetMoveSpeedBack;
        _animation.OnAttackRelease += SetMoveSpeedBack;

        _jumpCoroutine = JumpCoroutine();
        _chaseCoroutine = ChaseCoroutine();
    }
    private void SetMoveSpeedZero() => moveSpeed = 0;
    private void SetMoveSpeedBack() => moveSpeed = Managers.Data.Enemy[id].Speed;

    private void ActivateJumpShadow()
    {
        Shadows[(int)ShadowID.Default].SetActive(false);
        Shadows[(int)ShadowID.Jump].SetActive(true);
        Shadows[(int)ShadowID.Jump].transform.localScale = START_SHADOW_SCALE;
        Jump();

        Managers.Sound.Play(SoundID.SmollAmeJump);
    }
    private void DeActivateJumpShadow()
    {
        Shadows[(int)ShadowID.Default].SetActive(true);
        Shadows[(int)ShadowID.Jump].SetActive(false);
    }

    private void SetBodyColliderAttack()
    {
        _bodyCollider.offset = ATTACK_OFFSET;
        _bodyCollider.size = ATTACK_SIZE;
    }
    private void SetBodyColliderDefault()
    {
        _bodyCollider.offset = _defaultOffset;
        _bodyCollider.size = _defaultSize;
    }
    private void ActivateBodyCollider() => _bodyCollider.enabled = true;
    private void DeActivateBodyCollider() => _bodyCollider.enabled = false;
    private void ActivateAttackCollider() => _attackCollider.enabled = true;
    private void DeActivateAttackCollider() => _attackCollider.enabled = false;
    private void ActivateAttackEffect() => _attackEffect.SetActive(true);
    private void DeActivateAttackEffect() => _attackEffect.SetActive(false);

    private float _moveTime;
    private void Jump() => StartCoroutine(_jumpCoroutine);
    private IEnumerator _jumpCoroutine;
    private IEnumerator JumpCoroutine()
    {
        while (true)
        {
            _moveTime = 0;

            while (_moveTime < 0.5f)
            {
                _moveTime += Time.deltaTime;
                Shadows[(int)ShadowID.Jump].transform.localScale = Vector3.Lerp(START_SHADOW_SCALE, END_SHADOW_SCALE, _moveTime / 0.5f);

                yield return null;
            }

            StopCoroutine(_jumpCoroutine);

            StartCoroutine(_chaseCoroutine);

            yield return null;
        }
    }

    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private IEnumerator _chaseCoroutine;
    private IEnumerator ChaseCoroutine()
    {
        while (true)
        {
            _moveTime = 0;
            _startPoint = transform.position;
            _endPoint = Managers.Game.VTuber.transform.position;

            while (_moveTime < 0.5f)
            {
                _moveTime += Time.deltaTime;
                transform.position = Vector2.Lerp(_startPoint, _endPoint, _moveTime / 0.5f);

                yield return null;
            }

            moveSpeed = Managers.Data.Enemy[id].Speed / 3;

            yield return Util.DelayCache.GetWaitForSeconds(3);

            StopCoroutine(_chaseCoroutine);

            yield return null;
        }
    }

    private void ShakeCamera()
    {
        Util.CMCamera.Shake();
        Managers.Sound.Play(SoundID.SmollAmeAttack);
    }
}
