using UnityEngine;

public class MainTitleUI : UIBase
{
    [SerializeField] private GameObject _chars;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI -= ActivateMainTitleUI;
        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI += ActivateMainTitleUI;

        PresenterManager.TitleUIPresenter.OnDeActivateMainTitleUI -= DeActivateMainTitleUI;
        PresenterManager.TitleUIPresenter.OnDeActivateMainTitleUI += DeActivateMainTitleUI;
    }
    private void ActivateMainTitleUI()
    {
        _canvas.enabled = true;
        _chars.SetActive(true);
    }

    private void DeActivateMainTitleUI()
    {
        _canvas.enabled = false;
        _chars.SetActive(false);
    }
}
