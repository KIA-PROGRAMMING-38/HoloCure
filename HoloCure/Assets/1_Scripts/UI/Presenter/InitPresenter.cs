using Cysharp.Text;
using StringLiterals;
using System;
using UnityEngine;

namespace UI.Presenter
{
    public class InitPresenter
    {
        public event Action<Sprite> OnUpdatePortraitSprite;
        public event Action<Sprite> OnUpdateTitleSprite;
        public event Action<string> OnUpdateName;

        private void GetPortraitSprite(Sprite sprite) => OnUpdatePortraitSprite?.Invoke(sprite);
        private void GetTitleSprite(Sprite sprite) => OnUpdateTitleSprite?.Invoke(sprite);
        private void GetName(string name) => OnUpdateName?.Invoke(name);

        public void GetInitData(VTuberData data)
        {
            GetPortraitSprite(Managers.Resource.Sprites.Load(ZString.Concat(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.PortraitSprite)));
            GetTitleSprite(Managers.Resource.Sprites.Load(ZString.Concat(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.TitleSprite)));
            GetName(data.Name);
        }
    }
}