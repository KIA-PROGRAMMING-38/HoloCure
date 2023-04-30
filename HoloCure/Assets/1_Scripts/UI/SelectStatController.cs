using StringLiterals;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectStatController : UIBase
{
    [SerializeField] private SelectIconController _iconController;

    [SerializeField] private Image _titleImage;

    [SerializeField] private TextMeshProUGUI _nameText;

    // ���� ���� ������ �ִϸ��̼� Ŭ������ �����ϴ� ���
    [SerializeField] private Image _animation;

    [SerializeField] private TextMeshProUGUI[] _statTexts;

    private RectTransform _rectTransform;
    private Vector2 _initPos;
    private Vector2 _startPos;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initPos = _rectTransform.anchoredPosition;
        _startPos = _initPos + Vector2.left * 500;
    }
    private void Start()
    {
        _moveCoroutine = MoveCoroutine();

        _iconController.OnHoveredIcon -= GetVTuberIndex;
        _iconController.OnHoveredIcon += GetVTuberIndex;

        PresenterManager.TitleUIPresenter.OnActivateSelectUI -= StartMoveCoroutine;
        PresenterManager.TitleUIPresenter.OnActivateSelectUI += StartMoveCoroutine;
    }

    private void GetVTuberIndex(int index)
    {
        StartMoveCoroutine();

        // ���� ���� ������ ���̵� �޾Ƽ� �̺�Ʈ�� �߻���Ű�� �����͸� �����ͼ� ����

        if (index == 2)
        {
            _titleImage.enabled = true;
            _nameText.enabled = true;
            _animation.enabled = true;
            _statTexts[0].text = NumLiteral.GetNumString(75);
            _statTexts[1].text = "0.90";
            _statTexts[2].text = "1.50";
            _statTexts[3].text = NumLiteral.GetNumString(1);
        }
        else
        {
            _titleImage.enabled = false;
            _nameText.enabled = false;
            _animation.enabled = false;
            _statTexts[0].text = NumLiteral.GetNumString(0);
            _statTexts[1].text = NumLiteral.GetNumString(0);
            _statTexts[2].text = NumLiteral.GetNumString(0);
            _statTexts[3].text = NumLiteral.GetNumString(0);
        }
    }
    private float _elapsedTime;
    private void StartMoveCoroutine()
    {
        _elapsedTime = 0;
        StartCoroutine(_moveCoroutine);
    }
    private IEnumerator _moveCoroutine;
    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _initPos, _elapsedTime / 0.1f);

            if (_elapsedTime > 0.1f)
            {
                StopCoroutine(_moveCoroutine);
            }

            _elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
