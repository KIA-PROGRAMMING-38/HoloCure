using StringLiterals;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

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
        if (Input.GetButtonDown(InputLiteral.CANCEL))
        {
            OnCancel();
        }
    }

    private void OnCancel()
    {
        Managers.UI.OpenSubItem<PauseMainSubItem>(transform.parent).InitIndex(0);

        CloseSubItem();
    }
}