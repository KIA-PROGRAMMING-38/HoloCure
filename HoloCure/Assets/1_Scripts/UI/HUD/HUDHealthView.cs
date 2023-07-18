using TMPro;
using UnityEngine.UI;

public class HUDHealthView : View
{
    public Image GaugeImage { get; private set; }
    public TMP_Text CurHPText { get; private set; }
    public TMP_Text MaxHPText { get; private set; }
    protected override void Init()
    {
        GaugeImage = transform.Find("Gague Image").GetComponentAssert<Image>();
        CurHPText = transform.Find("CurHP Text").GetComponentAssert<TMP_Text>();
        MaxHPText = transform.Find("MaxHP Text").GetComponentAssert<TMP_Text>();
    }
}