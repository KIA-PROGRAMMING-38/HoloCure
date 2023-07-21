public class HudCountSubItem : UIBase
{
    #region Enums

    enum Texts
    {
        SecondText,
        MinuteText,
        CoinText,
        DefeatedEnemyText
    }

    #endregion

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        BindText(typeof(Texts));

        Managers.Game.Seconds.BindModelEvent(UpdateSecondsText, this);
        Managers.Game.Minutes.BindModelEvent(UpdateMinutesText, this);
        Managers.Game.Coins.BindModelEvent(UpdateCoinText, this);
        Managers.Game.DefeatedEnemies.BindModelEvent(UpdateDefeatedEnemyText, this);
    }

    private void UpdateSecondsText(int seconds)
    {
        GetText((int)Texts.SecondText).text = seconds.ToString("D2");
    }

    private void UpdateMinutesText(int minutes)
    {
        GetText((int)Texts.MinuteText).text = minutes.ToString("D2");
    }

    private void UpdateCoinText(int coin)
    {
        GetText((int)Texts.CoinText).text = coin.ToString();
    }

    private void UpdateDefeatedEnemyText(int enemy)
    {
        GetText((int)Texts.DefeatedEnemyText).text = enemy.ToString();
    }
}