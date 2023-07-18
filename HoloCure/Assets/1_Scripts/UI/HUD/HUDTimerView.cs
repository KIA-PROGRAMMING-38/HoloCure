using TMPro;

public class HUDTimerView : View
{
    public TMP_Text SecondsText { get; private set; }
    public TMP_Text MinutesText { get; private set; }
    protected override void Init()
    {
        SecondsText = transform.FindAssert("Seconds Text").GetComponentAssert<TMP_Text>();
        MinutesText = transform.FindAssert("Minutes Text").GetComponentAssert<TMP_Text>();
    }
}