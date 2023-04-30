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

    // 본래 설계 목적은 애니메이션 클립들을 변경하는 방식
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

        // 본래 설계 목적은 아이디를 받아서 이벤트를 발생시키고 데이터를 가져와서 설정

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
