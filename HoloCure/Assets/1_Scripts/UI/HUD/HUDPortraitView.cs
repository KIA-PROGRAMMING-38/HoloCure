using UnityEngine.UI;

public class HUDPortraitView : View
{
    public Image DisplayImage { get; private set; }
    protected override void Init()
    {
        DisplayImage = transform.FindAssert("Display Image").GetComponentAssert<Image>();
    }
}