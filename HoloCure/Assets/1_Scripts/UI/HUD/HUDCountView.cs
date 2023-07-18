using TMPro;

public class HUDCountView : View
{
    public TMP_Text CoinCountText {get; private set;}
    public TMP_Text DefeatedEnemyCountText { get; private set;}
    protected override void Init()
    {
        CoinCountText = transform.FindAssert("CoinCount Text").GetComponentAssert<TMP_Text>();
        DefeatedEnemyCountText = transform.FindAssert("DefeatedEnemyCount Text").GetComponentAssert<TMP_Text>();
    }
}