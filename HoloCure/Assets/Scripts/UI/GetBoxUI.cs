using UnityEngine;

public class GetBoxUI : UIBaseLegacy
{
    [SerializeField] private GameObject _boxAnimation;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {

    }
    private Sound _sound;
    private void ActivateGetBoxUI()
    {
        _canvas.enabled = true;
        _boxAnimation.SetActive(true);
        _sound = SoundPool.GetPlayAudio(SoundID.BoxOpen);
    }
    private void DeActivateGetBoxUI()
    {
        _canvas.enabled = false;
        _sound.StopPlaying();
    }
}
