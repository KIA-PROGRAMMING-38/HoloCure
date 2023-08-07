using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Util;

public class GetBoxOngoingSubItem : UISubItem
{
    public override void Init()
    {
        base.Init();

        this.UpdateAsObservable()
            .Subscribe(OnPressKey);

        Managers.Sound.Play(SoundID.BoxOpenOngoing);
    }

    private void OnPressKey(Unit unit)
    {
        if (Input.GetButtonDown(Define.Input.CONFIRM)|| Input.GetKeyDown(KeyCode.Mouse0))
        {
            OpenBox();
        }
    }

    public void OpenBox()
    {
        Managers.UI.OpenSubItem<GetBoxEndSubItem>(transform.parent);

        CloseSubItem();
    }
}