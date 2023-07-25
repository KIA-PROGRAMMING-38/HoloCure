using UnityEngine;

public class GetBoxPopup : UIPopup
{
    public override void Init()
    {
        base.Init();

        Managers.UI.OpenSubItem<GetBoxStartSubItem>(transform);

        Time.timeScale = 0;

        Managers.Sound.Pause(SoundType.BGM);
        Managers.Sound.BGMVolumeDown();
    }
}