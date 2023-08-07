using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Util;

public class PauseSkillsSubItem : UISubItem
{
    public override void Init()
    {
        base.Init();

        this.UpdateAsObservable()
            .Subscribe(OnKeyPress);
    }

    private void OnKeyPress(Unit unit)
    {
        if (Input.GetButtonDown(Define.Input.CANCEL))
        {
            OnCancel();
        }
    }

    private void OnCancel()
    {
        Managers.UI.OpenSubItem<PauseMainSubItem>(transform.parent).InitIndex(0);

        CloseSubItem();

        Managers.Sound.Play(SoundID.ButtonBack);
    }
}