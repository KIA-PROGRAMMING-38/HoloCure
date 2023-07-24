using Cysharp.Text;

public class StatSubItem : UISubItem
{
    #region Enums

    enum Images
    {
        TitleImage,
    }

    enum Texts
    {
        NameText,
        CurrentHPText,
        MaxHPText,
        AttackText,
        SpeedText,
        CriticalText,
        PickupText,
        HasteText
    }

    #endregion

    public override void Init()
    {
        base.Init();

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        SetupView();
    }

    private void SetupView()
    {
        VTuber vtuber = Managers.Game.VTuber;
        VTuberData data = Managers.Data.VTuber[vtuber.Id.Value];
        GetImage((int)Images.TitleImage).sprite = Managers.Resource.LoadSprite(data.TitleSprite);
        GetText((int)Texts.NameText).text = data.Name;

        GetText((int)Texts.CurrentHPText).text = vtuber.CurrentHp.Value.ToString();
        GetText((int)Texts.MaxHPText).text = vtuber.MaxHp.Value.ToString();
        GetText((int)Texts.AttackText).text = ZString.Concat("+", vtuber.AttackRate.Value, "%");
        GetText((int)Texts.SpeedText).text = ZString.Concat("+", vtuber.SpeedRate.Value, "%");
        GetText((int)Texts.CriticalText).text = ZString.Concat("+", vtuber.Critical.Value, "%");
        GetText((int)Texts.PickupText).text = ZString.Concat("+", vtuber.PickUpRate.Value, "%");
        GetText((int)Texts.HasteText).text = ZString.Concat("+", vtuber.Haste.Value, "%");
    }
}