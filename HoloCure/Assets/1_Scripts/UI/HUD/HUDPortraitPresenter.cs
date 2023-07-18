using UniRx;

public class HUDPortraitPresenter : Presenter
{
    private HUDPortraitView _portraitView;
    protected override void InitView()
    {
        _portraitView = transform.FindAssert("HUD Portrait View").GetComponentAssert<HUDPortraitView>();
    }

    protected override void OnRelease()
    {
        _portraitView = default;
    }
    protected override void OnUpdatedModel()
    {
        Managers.Game.VTuber.Id.Subscribe(UpdatePortraitImage).AddTo(CompositeDisposable);
    }
    private void UpdatePortraitImage(VTuberID id)
    {
        _portraitView.DisplayImage.sprite = Managers.Resource.LoadSprite(Managers.Data.VTuber[id].DisplaySprite);
    }
}