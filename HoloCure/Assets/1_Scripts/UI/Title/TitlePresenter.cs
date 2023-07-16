using StringLiterals;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitlePresenter : Presenter
{
    private TitleView _titleView;
    private int _index;
    protected override void InitView()
    {
        _titleView = transform.FindAssert("Title View").GetComponentAssert<TitleView>();
    }
    protected override void OnRelease()
    {
        _titleView = default;
    }
    protected override void OnOccuredUserEvent()
    {
        foreach (var button in _titleView.Buttons)
        {
            button.OnPointerDownAsObservable()
                .Subscribe(OnButtonDown).AddTo(CompositeDisposable);
            button.OnPointerEnterAsObservable()
                .Subscribe(_ => OnButtonEnter(_titleView.Buttons.IndexOf(button))).AddTo(CompositeDisposable);
        }

        this.UpdateAsObservable()
            .Subscribe(CheckKeyPress).AddTo(CompositeDisposable);
    }
    private void OnButtonDown(PointerEventData eventData)
    {
        MyButton button = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponentAssert<MyButton>();

        OnButtonDown(button);
    }
    private void OnButtonDown(MyButton button)
    {
        if (button == _titleView.PlayButton)
        {
            // Select UI와 연결
        }
        else if (button == _titleView.ShopButton) { return; }
        else if (button == _titleView.LeaderboardButton) { return; }
        else if (button == _titleView.AchievementsButton) { return; }
        else if (button == _titleView.SettingsButton) { return; }
        else if (button == _titleView.CreditsButton) { return; }
        else if (button == _titleView.QuitButton) { Application.Quit(); }
        else { Debug.Assert(false, $"Invalid Button: {button}"); }
    }
    private void OnButtonEnter(int index)
    {
        _index = index;
        for (int i = 0; i < _titleView.Buttons.Count; ++i)
        {
            if (i == _index)
            {
                _titleView.Buttons[i].ActivateHoveredFrame();
            }
            else
            {
                _titleView.Buttons[i].DeActivateHoveredFrame();
            }
        }
    }
    private void CheckKeyPress(Unit unit)
    {
        if (Input.GetButtonDown(InputLiteral.CONFIRM))
        {
            OnButtonDown(_titleView.Buttons[_index]);
        }
        else if (Input.GetButtonDown(InputLiteral.VERTICAL))
        {
            bool isUpKey = Input.GetAxisRaw(InputLiteral.VERTICAL) == 1;
            KeyMove(isUpKey);
        }

        void KeyMove(bool isUpKey)
        {
            int direction = isUpKey ? -1 : 1;
            _index += direction;
            if (_index < 0)
            {
                _index = 0;
            }
            if (_index >= _titleView.Buttons.Count)
            {
                _index = _titleView.Buttons.Count - 1;
            }
            OnButtonEnter(_index);
        }
    }
}