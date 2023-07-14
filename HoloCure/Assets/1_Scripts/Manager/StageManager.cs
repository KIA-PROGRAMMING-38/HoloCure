using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public event Action OnOneSecondPassed;
    public event Action OnPause;
    public event Action OnClear;

    public float CurrentStageTime;
    private int _elapsedSecond;
    private bool _isOnSelect;
    private bool _isOnPause;
    private void Start()
    {
        _stageCoroutine = StageCoroutine();
        _getKeyCoroutine = GetKeyCoroutine();
        _volumeUpCoroutine = VolumeUpCoroutine();
        _volumeDownCoroutine = VolumeDownCoroutine();
        _clearCoroutine = ClearCoroutine();

        OnOneSecondPassed -= Managers.PresenterM.TimePresenter.IncreaseOneSecond;
        OnOneSecondPassed += Managers.PresenterM.TimePresenter.IncreaseOneSecond;

        OnPause -= Managers.PresenterM.TriggerUIPresenter.ActivatePauseUI;
        OnPause += Managers.PresenterM.TriggerUIPresenter.ActivatePauseUI;
        Managers.PresenterM.TriggerUIPresenter.OnActivatePauseUI -= SetBoolOnPauseTrue;
        Managers.PresenterM.TriggerUIPresenter.OnActivatePauseUI += SetBoolOnPauseTrue;

        Managers.PresenterM.TriggerUIPresenter.OnResume -= SetBoolOnPauseFalse;
        Managers.PresenterM.TriggerUIPresenter.OnResume += SetBoolOnPauseFalse;
        Managers.PresenterM.TriggerUIPresenter.OnResume -= SetBoolOnSelectFalse;
        Managers.PresenterM.TriggerUIPresenter.OnResume += SetBoolOnSelectFalse;
        Managers.PresenterM.TriggerUIPresenter.OnResume -= StartGetKeyCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnResume += StartGetKeyCoroutine;

        Managers.PresenterM.TriggerUIPresenter.OnActivateLevelUpUI -= SetBoolOnSelectTrue;
        Managers.PresenterM.TriggerUIPresenter.OnActivateLevelUpUI += SetBoolOnSelectTrue;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGetBoxStartUI -= SetBoolOnSelectTrue;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGetBoxStartUI += SetBoolOnSelectTrue;

        Managers.PresenterM.TriggerUIPresenter.OnActivateLevelUpUI -= StopGetKeyCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnActivateLevelUpUI += StopGetKeyCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGetBoxStartUI -= StopGetKeyCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGetBoxStartUI += StopGetKeyCoroutine;

        Managers.PresenterM.TitleUIPresenter.OnPlayGameForStage -= StartStage;
        Managers.PresenterM.TitleUIPresenter.OnPlayGameForStage += StartStage;

        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= StopStage;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += StopStage;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= Managers.PresenterM.TimePresenter.ResetTimer;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += Managers.PresenterM.TimePresenter.ResetTimer;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += Managers.PresenterM.TitleUIPresenter.ActivateMainTitleUI;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += Managers.PresenterM.TitleUIPresenter.ActivateMainTitleUI;

        Managers.PresenterM.TriggerUIPresenter.OnActivatePauseUI -= StartVolumeDownCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnActivatePauseUI += StartVolumeDownCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnActivateLevelUpUI -= StartVolumeDownCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnActivateLevelUpUI += StartVolumeDownCoroutine;

        Managers.PresenterM.TriggerUIPresenter.OnResume -= StartVolumeUpCoroutine;
        Managers.PresenterM.TriggerUIPresenter.OnResume += StartVolumeUpCoroutine;

        Managers.PresenterM.TriggerUIPresenter.OnActivateGetBoxStartUI -= PauseBGM;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGetBoxStartUI += PauseBGM;
        Managers.PresenterM.TriggerUIPresenter.OnResume -= UnPauseBGM;
        Managers.PresenterM.TriggerUIPresenter.OnResume += UnPauseBGM;

        Managers.PresenterM.TriggerUIPresenter.OnActivateGameOverUI -= StopStage;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGameOverUI += StopStage;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGameClearUI -= StopStage;
        Managers.PresenterM.TriggerUIPresenter.OnActivateGameClearUI += StopStage;

        OnClear -= Managers.PresenterM.TriggerUIPresenter.ActivateGameClearUI;
        OnClear += Managers.PresenterM.TriggerUIPresenter.ActivateGameClearUI;

        _stageOneBGM = SoundPool.GetPlayAudio(SoundID.StageOneBGM);
        _stageOneBGM.SetVolume(Default_Volume);
        StopTitleBGM();
    }
    private void SetBoolOnPauseTrue() => _isOnPause = true;
    private void SetBoolOnPauseFalse() => _isOnPause = false;
    private void SetBoolOnSelectTrue() => _isOnSelect = true;
    private void SetBoolOnSelectFalse() => _isOnSelect = false;
    private void PauseBGM() => _stageOneBGM.Pause();
    private void UnPauseBGM() => _stageOneBGM.UnPause();
    private Sound _stageOneBGM;
    private const float Default_Volume = 0.6f;
    private const float Down_Volume = 0.2f;
    private float _volumeFixTime;
    private void StartVolumeUpCoroutine()
    {
        _volumeFixTime = 0f;
        StartCoroutine(_volumeUpCoroutine);
    }
    private IEnumerator _volumeUpCoroutine;
    private IEnumerator VolumeUpCoroutine()
    {
        while (true)
        {
            while (_volumeFixTime < 0.2f)
            {
                _stageOneBGM.SetVolume(Mathf.Lerp(Down_Volume, Default_Volume, _volumeFixTime / 0.2f));
                _volumeFixTime += Time.unscaledDeltaTime;
                yield return null;
            }

            StopCoroutine(_volumeUpCoroutine);

            yield return null;
        }
    }
    private void StartVolumeDownCoroutine()
    {
        _volumeFixTime = 0f;
        StartCoroutine(_volumeDownCoroutine);
    }
    private IEnumerator _volumeDownCoroutine;
    private IEnumerator VolumeDownCoroutine()
    {
        while (true)
        {
            while (_volumeFixTime < 0.2f)
            {
                _stageOneBGM.SetVolume(Mathf.Lerp(Default_Volume, Down_Volume, _volumeFixTime / 0.2f));
                _volumeFixTime += Time.unscaledDeltaTime;
                yield return null;
            }

            StopCoroutine(_volumeDownCoroutine);

            yield return null;
        }
    }
    private void PlayTitleBGM() => _stageOneBGM.gameObject.SetActive(true);
    private void StopTitleBGM()
    {
        _stageOneBGM.StopPlaying();
        _stageOneBGM.gameObject.SetActive(false);
    }
    private void StartStage()
    {
        CurrentStageTime = 0f;
        _elapsedSecond = 0;
        Time.timeScale = 1;
        _bossCount = 0;
        _isOnPause = false;
        _isOnSelect = false;

        PlayTitleBGM();

        StartCoroutine(_stageCoroutine);
        StartGetKeyCoroutine();

        Managers.Pool.OnIngameStart();
    }
    private void StopStage()
    {
        StopTitleBGM();
        StopCoroutine(_stageCoroutine);
        StopGetKeyCoroutine();

        Managers.Pool.OnOutgameStart();
    }
    private int _bossCount;
    public void CountBoss()
    {
        _bossCount += 1;
        if (_bossCount >= 2)
        {
            StartClearCoroutine();
        }
    }
    public void StartClearCoroutine()
    {
        _clearTime = 0;
        StartCoroutine(_clearCoroutine);
    }
    private float _clearTime;
    private IEnumerator _clearCoroutine;
    private IEnumerator ClearCoroutine()
    {
        while (true)
        {
            while (_clearTime < 1)
            {
                _clearTime += Time.unscaledDeltaTime;

                yield return null;
            }

            StopCoroutine(_clearCoroutine);
            StopStage();
            OnClear?.Invoke();

            yield return null;
        }
    }

    private IEnumerator _stageCoroutine;
    private IEnumerator StageCoroutine()
    {
        while (true)
        {
            CurrentStageTime += Time.deltaTime;

            if (CurrentStageTime >= _elapsedSecond + 1)
            {
                _elapsedSecond += 1;

                OnOneSecondPassed?.Invoke();
            }

            yield return null;
        }
    }
    private void StartGetKeyCoroutine()
    {
        _delayTime = 0;
        StartCoroutine(_getKeyCoroutine);
    }
    private void StopGetKeyCoroutine() => StopCoroutine(_getKeyCoroutine);
    private float _delayTime;
    private IEnumerator _getKeyCoroutine;
    private IEnumerator GetKeyCoroutine()
    {
        while (true)
        {
            while (_delayTime < 0.3f)
            {
                _delayTime += Time.unscaledDeltaTime;
                yield return null;
            }

            if (false == _isOnSelect && false == _isOnPause && Input.GetKeyDown(KeyCode.Escape))
            {
                StopGetKeyCoroutine();
                OnPause?.Invoke();
                yield return null;
            }

            yield return null;
        }
    }
}