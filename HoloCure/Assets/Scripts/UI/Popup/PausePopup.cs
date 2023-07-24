using UnityEngine;

public class PausePopup : UIPopup
{
    private StatSubItem _statSubItem;
    public override void Init()
    {
        base.Init();

        _statSubItem = Managers.UI.OpenSubItem<StatSubItem>(transform);
        Managers.UI.OpenSubItem<PauseMainSubItem>(transform);

        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        _statSubItem.CloseSubItem();
    }
}