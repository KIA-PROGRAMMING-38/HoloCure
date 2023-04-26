using System.Collections;
using UnityEngine;

public class SmollAme : Boss
{
    private SmollAmeAnimation _animation;
    private BoxCollider2D _defaultBodyCollider;
    private BoxCollider2D _attackBodyCollider;
    private CapsuleCollider2D _attackCollider;

    [SerializeField] private GameObject[] Shadows;
    private readonly Vector3 START_SHADOW_SCALE = new Vector3(26, 15.6f, 1);
    private readonly Vector3 END_SHADOW_SCALE = new Vector3(130, 78, 1);

    [SerializeField] private GameObject _attackPointer;
    protected override void Awake()
    {
        base.Awake();

        _animation = body.GetComponent<SmollAmeAnimation>();
        _defaultBodyCollider = GetComponent<BoxCollider2D>();
        _attackBodyCollider = _attackPointer.GetComponent<BoxCollider2D>();
        _attackCollider = _attackPointer.GetComponent<CapsuleCollider2D>();
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
        _animation.OnAttackEnd -= ActivateAttackBodyCollider;
        _animation.OnAttackEnd += ActivateAttackBodyCollider;

        _animation.OnAttackRelease -= DeActivateAttackBodyCollider;
        _animation.OnAttackRelease += DeActivateAttackBodyCollider;
        _animation.OnAttackRelease -= DeActivateAttackEffect;
        _animation.OnAttackRelease += DeActivateAttackEffect;
        _animation.OnAttackRelease -= SetMoveSpeedBack;
        _animation.OnAttackRelease += SetMoveSpeedBack;
        _animation.OnAttackRelease -= ActivateBodyCollider;
        _animation.OnAttackRelease += ActivateBodyCollider;

        _jumpCoroutine = JumpCoroutine();
        _chaseCoroutine = ChaseCoroutine();
    }
    private void SetMoveSpeedZero() => moveSpeed = 0;
    private void SetMoveSpeedBack() => moveSpeed = defaultSpeed;

    private void ActivateJumpShadow()
    {
        Shadows[0].SetActive(false);
        Shadows[1].SetActive(true);
        Shadows[1].transform.localScale = START_SHADOW_SCALE;
        Jump();
    }
    private void DeActivateJumpShadow()
    {
        Shadows[0].SetActive(true);
        Shadows[1].SetActive(false);
    }

    private void ActivateBodyCollider() => _defaultBodyCollider.enabled = true;
    private void DeActivateBodyCollider() => _defaultBodyCollider.enabled = false;
    private void ActivateAttackCollider() => _attackCollider.enabled = true;
    private void DeActivateAttackCollider() => _attackCollider.enabled = false;
    private void ActivateAttackBodyCollider() => _attackBodyCollider.enabled = true;
    private void DeActivateAttackBodyCollider() => _attackBodyCollider.enabled = false;
    private void ActivateAttackEffect() => _attackPointer.SetActive(true);
    private void DeActivateAttackEffect() => _attackPointer.SetActive(false);

    private float _moveTime;
    private void Jump() => StartCoroutine(_jumpCoroutine);
    private IEnumerator _jumpCoroutine;
    private IEnumerator JumpCoroutine()
    {
        while (true)
        {
            _moveTime = 0;

            while (_moveTime < 1)
            {
                _moveTime += Time.deltaTime;
                Shadows[1].transform.localScale = Vector3.Lerp(START_SHADOW_SCALE, END_SHADOW_SCALE, _moveTime);

                yield return null;
            }

            StopCoroutine(_jumpCoroutine);

            StartCoroutine(_chaseCoroutine);

            yield return null;
        }
    }

    Vector2 _startPoint;
    private IEnumerator _chaseCoroutine;
    private IEnumerator ChaseCoroutine()
    {
        while (true)
        {
            _moveTime = 0;
            _startPoint = transform.position;

            while (_moveTime < 1)
            {
                _moveTime += Time.deltaTime;
                transform.position = Vector2.Lerp(_startPoint, Util.Caching.CenterWorldPos, _moveTime);

                yield return null;
            }

            moveSpeed = defaultSpeed / 3;

            yield return Util.TimeStore.GetWaitForSeconds(3);

            StopCoroutine(_chaseCoroutine);

            yield return null;
        }
    }

    private void ShakeCamera() => Util.CMCamera.Shake(1, 10);
}
 