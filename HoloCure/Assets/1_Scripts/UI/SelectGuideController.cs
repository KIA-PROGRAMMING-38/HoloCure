using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectGuideController : UIBase
{
    [SerializeField] private SelectIconController _iconController;

    [SerializeField] private Image _weaponIcon;
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _weaponDescriptionText;

    private RectTransform _rectTransform;
    private Vector2 _initPos;
    private Vector2 _startPos;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initPos = _rectTransform.anchoredPosition;
        _startPos = _initPos + Vector2.right * 500;
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
            _weaponIcon.enabled = true;
            _weaponNameText.enabled = true;
            _weaponDescriptionText.enabled = true;
        }
        else
        {
            _weaponIcon.enabled = false;
            _weaponNameText.enabled = false;
            _weaponDescriptionText.enabled = false;
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
