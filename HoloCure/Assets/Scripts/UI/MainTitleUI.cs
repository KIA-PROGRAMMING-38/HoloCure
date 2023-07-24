using UnityEngine;

public class MainTitleUI : UIBaseLegacy
{
    [SerializeField] private GameObject _chars;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnGameEnd -= PlayTitleBGM;
        PresenterManager.TriggerUIPresenter.OnGameEnd += PlayTitleBGM;

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
    private void PlayTitleBGM() => _titleBGM.gameObject.SetActive(true);
    private void StopTitleBGM()
    {
        _titleBGM.StopPlaying();
        _titleBGM.gameObject.SetActive(false);
    }
}
