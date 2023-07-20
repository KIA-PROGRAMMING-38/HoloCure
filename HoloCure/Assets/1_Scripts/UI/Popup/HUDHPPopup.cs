public class HUDHPPopup : UIPopup
{
    #region Enums

    enum Images
    {
        HPGaugeImage
    }

    enum Texts
    {
        CurHPText,
        MaxHPText
    }

    #endregion

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Managers.Game.VTuber.CurHealth.BindModelEvent(UpdateHPGaugeImage, this);

        Managers.Game.VTuber.CurHealth.BindModelEvent(UpdateCurHPText, this);
        Managers.Game.VTuber.MaxHealth.BindModelEvent(UpdateMaxHPText, this);
    }

    private void UpdateHPGaugeImage(int CurHP)
    {
        float MaxHP = Managers.Game.VTuber.MaxHealth.Value;
        GetImage((int)Images.HPGaugeImage).fillAmount = CurHP / MaxHP;
    }

    private void UpdateCurHPText(int CurHP)
    {
        GetText((int)Texts.CurHPText).text = CurHP.ToString();
    }

    private void UpdateMaxHPText(int MaxHP)
    {
        GetText((int)Texts.MaxHPText).text = MaxHP.ToString();
    }
}