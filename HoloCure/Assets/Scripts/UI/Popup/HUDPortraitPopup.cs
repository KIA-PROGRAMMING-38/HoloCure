public class HudPortraitPopup : UIPopup
{
    #region Enums

    enum Images
    {
        PortraitDisplayImage
    }

    #endregion

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        BindImage(typeof(Images));

        Managers.Game.VTuber.Id.BindModelEvent(UpdatePortraitDisplayImage, this);
    }

    private void UpdatePortraitDisplayImage(VTuberID id)
    {
        VTuberData data = Managers.Data.VTuber[id];
        GetImage((int)Images.PortraitDisplayImage).sprite = Managers.Resource.LoadSprite(data.DisplaySprite);
    }
}