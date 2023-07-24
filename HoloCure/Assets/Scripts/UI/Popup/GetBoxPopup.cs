using UnityEngine;

public class GetBoxPopup : UIPopup
{
    public override void Init()
    {
        base.Init();

        Managers.UI.OpenSubItem<GetBoxStartSubItem>(transform);

        Time.timeScale = 0;
    }
}