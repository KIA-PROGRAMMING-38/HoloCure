using System.Collections.Generic;
public class TitleView : View
{
    public List<MyButton> Buttons { get; private set; }
    public MyButton PlayButton { get; private set; }
    public MyButton ShopButton { get; private set; }
    public MyButton LeaderboardButton { get; private set; }
    public MyButton AchievementsButton { get; private set; }
    public MyButton SettingsButton { get; private set; }
    public MyButton CreditsButton { get; private set; }
    public MyButton QuitButton { get; private set; }
    protected override void Init()
    {
        PlayButton = transform.FindAssert("Play Button").GetComponentAssert<MyButton>();
        ShopButton = transform.FindAssert("Shop Button").GetComponentAssert<MyButton>();
        LeaderboardButton = transform.FindAssert("Leaderboard Button").GetComponentAssert<MyButton>();
        AchievementsButton = transform.FindAssert("Achievements Button").GetComponentAssert<MyButton>();
        SettingsButton = transform.FindAssert("Settings Button").GetComponentAssert<MyButton>();
        CreditsButton = transform.FindAssert("Credits Button").GetComponentAssert<MyButton>();
        QuitButton = transform.FindAssert("Quit Button").GetComponentAssert<MyButton>();

        Buttons = new List<MyButton>
        {
            PlayButton,
            ShopButton,
            LeaderboardButton,
            AchievementsButton,
            SettingsButton,
            CreditsButton,
            QuitButton
        };
    }
}