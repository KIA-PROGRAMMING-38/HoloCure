using System.Collections.Generic;

public class HudPopup : UIPopup
{
    private List<UIBase> _subItems;
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        _subItems = new List<UIBase>()
        {
        Managers.UI.OpenSubItem<HudExpSubItem>(transform),
        Managers.UI.OpenSubItem<HudPortraitSubItem>(transform),
        Managers.UI.OpenSubItem<HudHpSubItem>(transform),
        Managers.UI.OpenSubItem<HudCountSubItem>(transform),
        Managers.UI.OpenSubItem<HudInventorySubItem>(transform),
        };
    }

    private void OnDestroy()
    {
        foreach (var subItem in _subItems)
        {
            Managers.Resource.Destroy(subItem.gameObject);
        }
    }
}