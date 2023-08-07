public enum ItemID
{
    None = 2000,
    CommonNone = 2100,
    SpiderCooking,
    HoloBomb,
    PsychoAxe,
    BLBook,
    FanBeam,
    StartingNone = 2200,
    SummonTentacle,
    ScytheSwing,
    PistolShot,
    TridentThrust,
    PhoenixSword,
    BirdFeather,
    Orbit,
    ClockHands,
    BrightStar,
    EquipmentNone = 2300,
    StatNone = 2400,
    MaxHPUp,
    ATKUp,
    SPDUp,
    CRTUp,
    PickUpRangeUp,
    HasteUp,
    End = 2500
}
public enum ItemType { None, Weapon, Equipment, Stat }
public enum WeaponType { None, Common, Starting }
public class ItemData
{
    public ItemID Id { get; set; }
    public string Name { get; set; }
    public string IconSprite { get; set; }
    public string Type { get; set; }
}
public class WeightData
{
    public ItemID Id { get; set; }
    public int Weight { get; set; }
}

public class WeaponLevelData
{
    public ItemID Id { get; set; }
    public int Level { get; set; }
    public string Description { get; set; }
    public float BaseAttackSequenceTime { get; set; }
    public float MinAttackSequenceTime { get; set; }
    public int StrikeCount { get; set; }
    public float DamageRate { get; set; }
    public float AttackDelay { get; set; }
    public float HitCoolTime { get; set; }
    public float Size { get; set; }
    public float AttackDurationTime { get; set; }
    public float ImpactDurationTime { get; set; }
    public float StrikeSpeed { get; set; }
    public float KnockbackDurationTime { get; set; }
    public float KnockbackSpeed { get; set; }
    public int Radius { get; set; }
    public float ImpactSize { get; set; }
}
public class StatData
{
    public ItemID Id { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
}
