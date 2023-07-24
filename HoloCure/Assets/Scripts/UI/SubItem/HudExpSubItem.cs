public class HudExpSubItem : UISubItem
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
        base.Init();

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Managers.Game.VTuber.CurrentExp.BindModelEvent(UpdateExpGaugeImage, this);
        Managers.Game.VTuber.Level.BindModelEvent(UpdateLevelText, this);
    }

    private void UpdateExpGaugeImage(int currentExp)
    {
        float maxExp = Managers.Game.VTuber.MaxExp.Value;
        GetImage((int)Images.ExpGaugeImage).fillAmount = currentExp / maxExp;
    }

    private void UpdateLevelText(int level)
    {
        GetText((int)Texts.LevelText).text = level.ToString();
    }
}