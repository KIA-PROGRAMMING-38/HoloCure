using UnityEngine;
using UnityEngine.UI;

public class HUDPortraitImage : UIBase
{
    private Image _image;
    private void Awake() => _image = GetComponent<Image>();
    private void Start()
    {
        PresenterManager.InitPresenter.OnUpdatePortraitSprite -= UpdatePortrait;
        PresenterManager.InitPresenter.OnUpdatePortraitSprite += UpdatePortrait;
    }
    private void UpdatePortrait(Sprite sprite) => _image.sprite = sprite;
}
