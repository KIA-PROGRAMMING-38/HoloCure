using TMPro;
using UnityEngine.UI;

public class HUDExpView : View
{
    public Image GaugeImage { get; private set; }
    public TMP_Text LevelText { get; private set; }
    protected override void Init()
    {
        GaugeImage = transform.FindAssert("Gauge Image").GetComponentAssert<Image>();
        LevelText = transform.FindAssert("Level Text").GetComponentAssert<TMP_Text>();
    }
}