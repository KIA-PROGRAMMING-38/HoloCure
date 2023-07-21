public class HudHpSubItem : UIBase
{
    #region Enums

    enum Images
    {
        HpGaugeImage
    }

    enum Texts
    {
        CurrentHpText,
        MaxHpText
    }

    #endregion

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Managers.Game.VTuber.CurrentHp.BindModelEvent(UpdateHPGaugeImage, this);

        Managers.Game.VTuber.CurrentHp.BindModelEvent(UpdateCurHPText, this);
        Managers.Game.VTuber.MaxHp.BindModelEvent(UpdateMaxHPText, this);
    }

    private void UpdateHPGaugeImage(int currentHp)
    {
        float maxHp = Managers.Game.VTuber.MaxHp.Value;
        GetImage((int)Images.HpGaugeImage).fillAmount = currentHp / maxHp;
    }

    private void UpdateCurHPText(int currentHp)
    {
        GetText((int)Texts.CurrentHpText).text = currentHp.ToString();
    }

    private void UpdateMaxHPText(int maxHp)
    {
        GetText((int)Texts.MaxHpText).text = maxHp.ToString();
    }
}