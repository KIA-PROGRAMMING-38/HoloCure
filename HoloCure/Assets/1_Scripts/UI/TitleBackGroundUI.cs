    using UnityEngine;

public class TitleBackGroundUI : UIBaseLegacy
{
    [SerializeField] private GameObject _triangles;
    [SerializeField] private GameObject _bars;
    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TitleUIPresenter.OnActivateTitleBackGroundUI -= ActivateTitleBackGroundUI;
        PresenterManager.TitleUIPresenter.OnActivateTitleBackGroundUI += ActivateTitleBackGroundUI;

        PresenterManager.TitleUIPresenter.OnDeactivateTitleBackGroundUI -= DeActivateTitleBackGroundUI;
        PresenterManager.TitleUIPresenter.OnDeactivateTitleBackGroundUI += DeActivateTitleBackGroundUI;
    }
    private void ActivateTitleBackGroundUI()
    {
        _canvas.enabled = true;
        _triangles.SetActive(true);
        _bars.SetActive(true);
    }

    private void DeActivateTitleBackGroundUI()
    {
        _canvas.enabled = false;
        _triangles.SetActive(false);
        _bars.SetActive(false);
    }
}
