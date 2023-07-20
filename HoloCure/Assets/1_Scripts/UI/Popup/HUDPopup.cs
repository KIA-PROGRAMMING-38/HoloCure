public class HUDPopup : UIPopup
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        Managers.UI.OpenPopupUI<HUDExpPopup>();
        Managers.UI.OpenPopupUI<HUDPortraitPopup>();
        Managers.UI.OpenPopupUI<HUDHPPopup>();
        Managers.UI.OpenPopupUI<HUDCountPopup>();
        Managers.UI.OpenPopupUI<HUDInventoryPopup>();
    }
}