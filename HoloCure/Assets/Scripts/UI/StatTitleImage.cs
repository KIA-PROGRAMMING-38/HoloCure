using UnityEngine;
using UnityEngine.UI;

public class StatTitleImage : UIBaseLegacy
{
    private Image _image;
    private void Awake() => _image = GetComponent<Image>();
    private void Start()
    {
        PresenterManager.InitPresenter.OnUpdateTitleSprite -= UpdateTitle;
        PresenterManager.InitPresenter.OnUpdateTitleSprite += UpdateTitle;
    }
    private void UpdateTitle(Sprite sprite) => _image.sprite = sprite;
}
