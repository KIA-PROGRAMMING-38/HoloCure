using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] _audioSources;

    public void Init()
    {
        _audioSources = new AudioSource[(int)SoundType.Max];

        string[] soundTypeNames = Enum.GetNames(typeof(SoundType));
        for (int i = 0; i < _audioSources.Length; ++i)
        {
            GameObject go = new GameObject(soundTypeNames[i]);
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = transform;
        }

        _audioSources[(int)SoundType.BGM].loop = true;
    }

    public void StopAll()
    {
        foreach (var audioSource in _audioSources)
        {
            audioSource.Stop();
        }
    }

    public void Play(SoundID id)
    {
        SoundData data = Managers.Data.Sound[id];
        SoundType type = id.GetSoundType();

        AudioSource audioSource = _audioSources[(int)type];
        audioSource.volume = data.Volume;
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

    private const float FADE_DURATION = 0.15f;
    private const float MIN_VOLUME = 0.15f;
    private IEnumerator _changeVolumeCo;
    public void BGMVolumeUp()
    {
        ChangeVolume(SoundType.BGM, true);
    }

    public void BGMVolumeDown()
    {
        ChangeVolume(SoundType.BGM, false);
    }

    private void ChangeVolume(SoundType type, bool increaseVolume)
    {
        AudioSource audioSource = _audioSources[(int)type];
        if (_changeVolumeCo != null)
        {
            StopCoroutine(_changeVolumeCo);
        }
        _changeVolumeCo = ChangeVolumeCo(audioSource, increaseVolume);
        StartCoroutine(_changeVolumeCo);
    }

    private IEnumerator ChangeVolumeCo(AudioSource audioSource, bool increaseVolume)
    {
        SoundID currentStageBGM = SoundID.StageOneBGM + Managers.Game.Stage.Value - 1;
        float initialVolume = Managers.Data.Sound[currentStageBGM].Volume;
        float targetVolume = increaseVolume ? initialVolume : MIN_VOLUME;
        float startVolume = increaseVolume ? MIN_VOLUME : initialVolume;
        float time = 0f;

        while (time < FADE_DURATION)
        {
            time += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / FADE_DURATION);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    public float GetAudioClipLength(string path) => GetAudioClip(path).length;
    private AudioClip GetAudioClip(string path) => Managers.Resource.LoadAudioClip(path);
}