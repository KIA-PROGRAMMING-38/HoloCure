using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] _audioSources = new AudioSource[(int)SoundType.Max];

    public void Init()
    {
        string[] soundTypeNames = Enum.GetNames(typeof(SoundType));
        for (int i = 0; i < _audioSources.Length; ++i)
        {
            GameObject go = new GameObject(soundTypeNames[i]);
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = transform;
        }

        _audioSources[(int)SoundType.BGM].loop = true;

        _volumeUpCo = VolumeUpCo();
        _volumeDownCo = VolumeDownCo();
    }

    public void Play(SoundID id)
    {
        SoundData data = Managers.Data.Sound[id];
        SoundType type = id.GetSoundType();

        AudioSource audioSource = _audioSources[(int)type];
        AudioClip audioClip = GetAudioClip(data.Name);

        if (type == SoundType.Effect)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void Stop(SoundType type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Stop();
    }

    public void Pause(SoundType type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Pause();
    }

    public void UnPause(SoundType type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.UnPause();
    }

    private AudioSource _fadeSource;
    private float _fadeTime;
    private const float FADE_DURATION = 0.2f;
    private const float FADE_UP = 0.6f;
    private const float FADE_DOWN = 0.2f;
    private IEnumerator _volumeUpCo;
    private IEnumerator _volumeDownCo;
    public void VolumeUp(SoundType type)
    {
        _fadeSource = _audioSources[(int)type];
        _fadeTime = 0;
        StartCoroutine(_volumeUpCo);
    }

    public void VolumeDown(SoundType type)
    {
        _fadeSource = _audioSources[(int)type];
        _fadeTime = 0;
        StartCoroutine(_volumeDownCo);
    }

    private IEnumerator VolumeUpCo()
    {
        while (true)
        {
            while (_fadeTime < FADE_DURATION)
            {
                _fadeSource.volume = Mathf.Lerp(FADE_DOWN, FADE_UP, _fadeTime / FADE_DURATION);
                _fadeTime += Time.unscaledDeltaTime;
                yield return null;
            }

            StopCoroutine(_volumeUpCo);
            yield return null;
        }
    }

    public IEnumerator VolumeDownCo()
    {
        while (_fadeTime < FADE_DURATION)
        {
            _fadeSource.volume = Mathf.Lerp(FADE_UP, FADE_DOWN, _fadeTime / FADE_DURATION);
            _fadeTime += Time.unscaledDeltaTime;
            yield return null;
        }

        StopCoroutine(_volumeDownCo);
        yield return null;
    }

    public float GetAudioClipLength(string path) => GetAudioClip(path).length;
    private AudioClip GetAudioClip(string path) => Managers.Resource.LoadAudioClip(path);
}