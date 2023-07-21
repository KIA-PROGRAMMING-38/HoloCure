public class HudPopup : UIPopup
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        Managers.UI.OpenPopupUI<HudExpPopup>();
        Managers.UI.OpenPopupUI<HudPortraitPopup>();
        Managers.UI.OpenPopupUI<HudHpPopup>();
        Managers.UI.OpenPopupUI<HudCountPopup>();
        Managers.UI.OpenPopupUI<HudInventoryPopup>();
    }
}