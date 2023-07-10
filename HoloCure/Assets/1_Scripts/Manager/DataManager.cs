using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;

public enum VTuberID
{
    None = 10000,
    Ina,
}
public class VTuberData
{
    public VTuberID Id { get; set; }
    public string Name { get; set; }
    public string DisplaySprite { get; set; }
    public string PortraitSprite { get; set; }
    public string TitleSprite { get; set; }
    public int Health { get; set; }
    public float ATK { get; set; }
    public float SPD { get; set; }
    public float CRT { get; set; }
    public float PickUp { get; set; }
    public float Haste { get; set; }
}
public enum EnemyID
{
    None = 1000,
    Shrimp,
    Deadbeat,
    Takodachi,
    KFP,
    ShrimpDark,
    Gloom,
    Bloom,
    DeadbeatBatter,
    InvestiGator,
    TakodachiAngry,
    KFPAngry,
    Baerat,
    KromieA,
    KromieB,
    ShrimpGangA,
    DeadbeatGangA,
    SaplingA,
    SaplingB,
    SaplingC,
    HoomanA,
    HoomanB,
    BreadDog,
    SaplingKing,
    KromieKing,
    Bubba,
    MiniBossNone = 2000,
    Boss_Shrimp,
    Boss_Takodachi,
    Boss_ShrimpDark,
    Boss_DeadbeatBatter,
    Boss_KromieKing,
    Boss_ShrimpGangA,
    Boss_DeadbeatGangA,
    Boss_SaplingKing,
    BossNone = 3000,
    Boss_Fubuzilla,
    Boss_SmolAme,
}
public class EnemyData
{
    public EnemyID Id { get; set; }
    public string Name { get; set; }
    public string Sprite { get; set; }
    public int Health { get; set; }
    public int ATK { get; set; }
    public float SPD { get; set; }
    public int Exp { get; set; }
    public float Scale { get; set; }
    public int SpawnStartTime { get; set; }
    public int SpawnEndTime { get; set; }
}
public enum ItemID
{
    CommonNone = 5000,
    SpiderCooking,
    HoloBomb,
    PsychoAxe,
    BLBook,
    FanBeam,
    StartingNone = 6000,
    SummonTentacle,
    StatNone = 9000,
    MaxHPUp,
    ATKUp,
    SPDUp,
    CRTUp,
    PickUpRangeUp,
    HasteUp,
}
public class ItemData
{
    public ItemID Id { get; set; }
    public string Name { get; set; }
    public string IconSprite { get; set; }
    public string Type { get; set; }
    public int Weight { get; set; }
}

public class WeaponData
{
    public ItemID Id { get; set; }
    public int Level { get; set; }
    public string Description { get; set; }
    public float BaseAttackSequenceTime { get; set; }
    public float MinAttackSequenceTime { get; set; }
    public int ProjectileCount { get; set; }
    public float DamageRate { get; set; }
    public float AttackDelay { get; set; }
    public float HitCoolTime { get; set; }
    public float Size { get; set; }
    public float AttackDurationTime { get; set; }
    public int ProjectileSpeed { get; set; }
    public float KnockbackDurationTime { get; set; }
    public float KnockbackSpeed { get; set; }
    public int Radius { get; set; }
}
public class StatData
{
    public ItemID Id { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
}

public enum MaterialID
{
    None,
    Hit,
}
public class MaterialData
{
    public MaterialID Id { get; set; }
    public string Material { get; set; }
}
public class DataManager
{
    // NOTE : 데이터테이블 추가해야함.
    // NOTE : 느슨한 식별자의 경우 List를, 엄격한 식별자의 경우 Dictionary 사용.
    // NOTE : Id를 열거형으로 만들어두면 오류낼 일이 적음
    public Dictionary<VTuberID, VTuberData> VTuber { get; private set; }
    public Dictionary<EnemyID, EnemyData> Enemy { get; private set; }
    public Dictionary<ItemID, ItemData> Item { get; private set; }
    public Dictionary<ItemID, Dictionary<int, WeaponData>> Weapon { get; private set; }
    public Dictionary<ItemID, StatData> Stat { get; private set; }
    public Dictionary<MaterialID, MaterialData> Material { get; private set; }
    public void Init()
    {
        VTuber = ParseToDict<VTuberID, VTuberData>("Assets/Resources/4_DataTable/VTuber.csv", data => data.Id);
        Enemy = ParseToDict<EnemyID, EnemyData>("Assets/Resources/4_DataTable/Enemy.csv", data => data.Id);
        Item = ParseToDict<ItemID, ItemData>("Assets/Resources/4_DataTable/Item.csv", data => data.Id);

        Weapon = new();
        List<WeaponData> wpList = ParseToList<WeaponData>("Assets/Resources/4_DataTable/Weapon.csv");
        foreach (var wpData in wpList)
        {
            if (false == Weapon.ContainsKey(wpData.Id))
            {
                Weapon[wpData.Id] = new();
            }

            Weapon[wpData.Id][wpData.Level] = wpData;
        }

        Stat = ParseToDict<ItemID, StatData>("Assets/Resources/4_DataTable/Stat.csv", data => data.Id);
        Material = ParseToDict<MaterialID, MaterialData>("Assets/Resources/4_DataTable/Material.csv", data => data.Id);
    }
    private List<T> ParseToList<T>([NotNull] string path)
    {
        using var reader = new StreamReader(path);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csv.GetRecords<T>().ToList();
    }

    private Dictionary<Key, Item> ParseToDict<Key, Item>([NotNull] string path, Func<Item, Key> keySelector)
    {
        using var reader = new StreamReader(path);

        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csv.GetRecords<Item>().ToDictionary(keySelector);
    }
}