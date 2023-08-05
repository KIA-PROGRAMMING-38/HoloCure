public class SoundData
{
    public SoundID Id { get; set; }
    public string Name { get; set; }
    public float Volume { get; set; }
}
public enum SoundID
{
    None = 3000,
    BGM = 3100,
    TitleBGM,
    StageOneBGM,
    Common = 3200,
    GameOver,
    GameClear,
    BoxOpenStart,
    BoxOpenOngoing,
    BoxOpenEnd,
    Effect = 3300,
    ButtonMove,
    ButtonClick,
    ButtonBack,
    SelectMove,
    SelectClick,
    PlayerDamaged,
    EnemyDamaged,
    GetExp,
    LevelUp,
    SmollAmeJump,
    SmollAmeAttack,
    FanBeam,
    HoloBomb,
    PsychoAxe,
    SummonTentacle,
    ScytheSwing,
    PistolShot,
    TridentThrust,
    PhoenixSword,
    BirdFeather,
    Orbit,
    ClockHands,
    BrightStar,
    FoxTail,
    TarotCards,
    Orayo,
    DivaSong,
    HiSpecPistol,
    End = 3400
}
public enum SoundType { BGM, Common, Effect, Max };