using UnityEngine;

public class MainTitleUI : UIBase
{
    [SerializeField] private GameObject _chars;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI -= ActivateMainTitleUI;
        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI += ActivateMainTitleUI;

        PresenterManager.TitleUIPresenter.OnDeActivateMainTitleUI -= DeActivateMainTitleUI;
        PresenterManager.TitleUIPresenter.OnDeActivateMainTitleUI += DeActivateMainTitleUI;

        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI -= PlayTitleBGM;
        PresenterManager.TitleUIPresenter.OnActivateMainTitleUI += PlayTitleBGM;
        PresenterManager.TitleUIPresenter.OnPlayGame -= StopTitleBGM;
        PresenterManager.TitleUIPresenter.OnPlayGame += StopTitleBGM;

        _titleBGM = SoundPool.GetPlayAudio(SoundID.TitleBGM);
        _titleBGM.SetVolume(0.6f);
    }
    private void ActivateMainTitleUI()
    {
        _canvas.enabled = true;
        _chars.SetActive(true);
    }

    private void DeActivateMainTitleUI()
    {
        _canvas.enabled = false;
        _chars.SetActive(false);
    }
    private Sound _titleBGM;
    private void PlayTitleBGM()
    {
        if (_titleBGM.IsPlaying())
        {
            return;
        }

        _titleBGM = SoundPool.GetPlayAudio(SoundID.TitleBGM);
        _titleBGM.SetVolume(0.6f);
    }
    private void StopTitleBGM() => _titleBGM.StopPlaying();
}
