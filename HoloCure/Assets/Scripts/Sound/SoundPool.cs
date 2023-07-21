using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Util.Pool;

public enum SoundID
{
    None,
    TitleBGM,
    StageOneBGM,
    GameOver,
    GameClear,
    BoxOpenStart,
    BoxOpen,
    BoxOpenEnd,
    ButtonMove,
    ButtonClick,
    ButtonBack,
    CharMove,
    CharClick,
    SummonTentacle,
    FanBeam,
    HoloBomb,
    PsychoAxe,
    PlayerDamaged,
    EnemyDamaged,
    GetExp,
    LevelUp,
    SmollAmeJump,
    SmollAmeAttack
}

public static class SoundPool
{
    private static Dictionary<SoundID, AudioClip> _audioClipContainer = new();
    private static ObjectPool<Sound> _soundPool;

    public static AudioClip GetAudioClip(SoundID ID) => _audioClipContainer[ID];
    public static Sound GetPlayAudio(SoundID ID)
    {
        Sound sound = _soundPool.Get();
        AudioClip clip = _audioClipContainer[ID];
        sound.Clip = clip;

        sound.gameObject.SetActive(false);
        sound.gameObject.SetActive(true);

        return sound;
    }
    public static void Release(Sound sound) => _soundPool.Release(sound);
    private static Sound CreateSound()
    {
        GameObject gameObject = new();
        gameObject.AddComponent<AudioSource>();
        Sound sound = gameObject.AddComponent<Sound>();

        return sound;
    }
    private static void OnGetSoundFromPool(Sound sound) => sound.gameObject.SetActive(true);
    private static void OnReleaseSoundToPool(Sound sound) => sound.gameObject.SetActive(false);
    private static void OnDestroySound(Sound sound) => Object.Destroy(sound.gameObject);

    static SoundPool()
    {
        _soundPool = new ObjectPool<Sound>(CreateSound, OnGetSoundFromPool, OnReleaseSoundToPool, OnDestroySound);

        // 배경음
        _audioClipContainer.Add(SoundID.TitleBGM, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "bgm_SSS")));
        _audioClipContainer.Add(SoundID.StageOneBGM, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "bgm_suspect")));
        _audioClipContainer.Add(SoundID.GameOver, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "bgm_gameover")));
        _audioClipContainer.Add(SoundID.GameClear, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "bgm_gamewin")));
        
        // 박스 효과음
        _audioClipContainer.Add(SoundID.BoxOpenStart, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_delivery")));
        _audioClipContainer.Add(SoundID.BoxOpen, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "bgm_chestopen1")));
        _audioClipContainer.Add(SoundID.BoxOpenEnd, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "bgm_chestopen1_short")));

        // 메뉴 효과음
        _audioClipContainer.Add(SoundID.ButtonMove, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_menu_select")));
        _audioClipContainer.Add(SoundID.ButtonClick, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_menu_confirm")));
        _audioClipContainer.Add(SoundID.ButtonBack, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_menu_back")));
        _audioClipContainer.Add(SoundID.CharMove, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_charSelectWoosh")));
        _audioClipContainer.Add(SoundID.CharClick, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_charSelected")));

        // 무기 효과음
        _audioClipContainer.Add(SoundID.SummonTentacle, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_tentacle")));
        _audioClipContainer.Add(SoundID.FanBeam, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_laser")));
        _audioClipContainer.Add(SoundID.HoloBomb, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_feathers")));
        _audioClipContainer.Add(SoundID.PsychoAxe, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_normalthrow")));

        // 피격음
        _audioClipContainer.Add(SoundID.PlayerDamaged, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_hurt")));
        _audioClipContainer.Add(SoundID.EnemyDamaged, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_hit1")));

        // 경험치 획득 효과음
        _audioClipContainer.Add(SoundID.GetExp, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_getEXP")));

        // 레벨업
        _audioClipContainer.Add(SoundID.LevelUp, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_lvlup")));

        // 스몰아메 효과음
        _audioClipContainer.Add(SoundID.SmollAmeJump, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_amejump")));
        _audioClipContainer.Add(SoundID.SmollAmeAttack, Resources.Load<AudioClip>(Path.Combine(PathLiteral.SOUND, "snd_amegroundpound")));

    }
}