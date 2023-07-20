using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class BoxAnimation : UIBaseLegacy
{
    public event Action OnOpen;

    private RectTransform _rectTransform;
    private Animator _animator;
    private readonly Vector3 OPEN_BOX_SCALE = new(1.3f, 1.9f, 1);
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _getKeyCoroutine = GetKeyCoroutine();

        PresenterManager.TriggerUIPresenter.OnActivateGetBoxUI -= StartGetKeyCoroutine;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxUI += StartGetKeyCoroutine;

        OnOpen -= PresenterManager.TriggerUIPresenter.ActivateGetBoxEndUI;
        OnOpen += PresenterManager.TriggerUIPresenter.ActivateGetBoxEndUI;

        gameObject.SetActive(false);
    }
    public void StartGetKeyCoroutine()
    {
        _delayTime = 0;
        StartCoroutine(_getKeyCoroutine);
    }
    private float _delayTime;
    private IEnumerator _getKeyCoroutine;
    private IEnumerator GetKeyCoroutine()
    {
        while (true)
        {
            while (_delayTime < 0.3f)
            {
                _delayTime += Time.unscaledDeltaTime;
                yield return null;
            }

            if (Input.GetButtonDown(InputLiteral.CONFIRM) || Input.GetKeyDown(KeyCode.Mouse0) ||
                _delayTime > _animator.runtimeAnimatorController.animationClips[0].length)
            {
                Open();
                yield return null;
            }

            _delayTime += Time.unscaledDeltaTime;

            yield return null;
        }
    }
    /// <summary>
    /// 박스 스프라이트의 크기가 달라 만들어진 함수입니다. 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void SetScaleOpenBox() => _rectTransform.localScale = OPEN_BOX_SCALE;
    /// <summary>
    /// 박스가 오픈되는순간 이벤트를 발생시키기 위한 함수입니다. 애니메이션 이벤트에서 또는 컨펌시 호출됩니다.
    /// </summary>
    public void Open()
    {
        StopCoroutine(_getKeyCoroutine);
        OnOpen?.Invoke();
        _rectTransform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }
}
