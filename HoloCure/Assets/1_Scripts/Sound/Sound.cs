using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip Clip
    {
        set
        {
            _clipTime = value.length;
            _audioSource.clip = value;
        }
    }

    private AudioSource _audioSource;
    private float _elapsedTime = 0f;
    private float _clipTime = 0f;
    private bool _isReleased = false;
    private bool _isPause = false;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _elapsedTime = 0f;
        _isReleased = false;
        _audioSource.Play();
    }

    private void Update()
    {
        if (_isPause)
        {
            return;
        }

        _elapsedTime += Time.unscaledDeltaTime;

        if (_clipTime <= _elapsedTime)
        {
            RepeatBGM();
            if (_elapsedTime == 0)
            {
                return;
            }

            _isReleased = true;
            SoundPool.Release(this);
        }
    }
    private void RepeatBGM()
    {
        if (_audioSource.clip == SoundPool.GetAudioClip(SoundID.StageOneBGM) ||
           _audioSource.clip == SoundPool.GetAudioClip(SoundID.TitleBGM))
        {
            _audioSource.Stop();
            _audioSource.Play();
            _elapsedTime = 0;
        }
    }
    public void Pause() => _audioSource.Pause();
    public void UnPause() => _audioSource.UnPause();
    public void StopPlaying()
    {
        _audioSource.Stop();
        _isReleased = true;
        SoundPool.Release(this);
    }
    public bool IsPlaying() => _isReleased == false;
    public void SetVolume(float value) => _audioSource.volume = value;
}