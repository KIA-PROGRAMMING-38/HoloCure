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
    /// �ڽ� ��������Ʈ�� ũ�Ⱑ �޶� ������� �Լ��Դϴ�. �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    /// </summary>
    public void SetScaleOpenBox() => _rectTransform.localScale = OPEN_BOX_SCALE;
    /// <summary>
    /// �ڽ��� ���µǴ¼��� �̺�Ʈ�� �߻���Ű�� ���� �Լ��Դϴ�. �ִϸ��̼� �̺�Ʈ���� �Ǵ� ���߽� ȣ��˴ϴ�.
    /// </summary>
    public void Open()
    {
        StopCoroutine(_getKeyCoroutine);
        OnOpen?.Invoke();
        _rectTransform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }
}
