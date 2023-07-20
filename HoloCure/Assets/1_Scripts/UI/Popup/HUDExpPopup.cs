public class HUDExpPopup : UIPopup
{
    #region Enums

    enum Images
    {
        ExpGaugeImage
    }

    enum Texts
    {
        LevelText
    }

    #endregion

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Managers.Game.VTuber.CurExp.BindModelEvent(UpdateExpGaugeImage, this);
        Managers.Game.VTuber.Level.BindModelEvent(UpdateLevelText, this);
    }

    private void UpdateExpGaugeImage(int curExp)
    {
        float maxExp = Managers.Game.VTuber.MaxExp.Value;
        GetImage((int)Images.ExpGaugeImage).fillAmount = curExp / maxExp;
    }

    private void UpdateLevelText(int level)
    {
        GetText((int)Texts.LevelText).text = level.ToString();
    }
}