using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;

public class DataManager
{
    public Dictionary<VTuberID, VTuberData> VTuber { get; private set; }
    public Dictionary<EnemyID, EnemyData> Enemy { get; private set; }
    public Dictionary<ItemID, ItemData> Item { get; private set; }
    public List<WeightData> WeaponWeight { get; private set; }
    public List<WeightData> StatWeight { get; private set; }
    public Dictionary<ItemID, List<WeaponLevelData>> WeaponLevelTable { get; private set; }
    public Dictionary<ItemID, StatData> Stat { get; private set; }
    public Dictionary<MaterialID, MaterialData> Material { get; private set; }
    public List<ExpData> Exp { get; private set; }
    public List<SelectIdolData> SelectIdol { get; private set; }
    public List<SelectStageData> SelectStage { get; private set; }
    public void Init()
    {
        VTuber = ParseToDict<VTuberID, VTuberData>("Assets/Resources/4_DataTable/VTuber.csv", data => data.Id);
        Enemy = ParseToDict<EnemyID, EnemyData>("Assets/Resources/4_DataTable/Enemy.csv", data => data.Id);
        Item = ParseToDict<ItemID, ItemData>("Assets/Resources/4_DataTable/Item.csv", data => data.Id);
        WeaponWeight = ParseToList<WeightData>("Assets/Resources/4_DataTable/WeaponWeight.csv");
        StatWeight = ParseToList<WeightData>("Assets/Resources/4_DataTable/StatWeight.csv");

        WeaponLevelTable = new();
        List<WeaponLevelData> wpList = ParseToList<WeaponLevelData>("Assets/Resources/4_DataTable/WeaponLevelTable.csv");
        foreach (var wpData in wpList)
        {
            if (false == WeaponLevelTable.ContainsKey(wpData.Id))
            {
                WeaponLevelTable[wpData.Id] = new List<WeaponLevelData> { new WeaponLevelData() };
                WeaponLevelTable[wpData.Id][0].Id = wpData.Id;
            }

            WeaponLevelTable[wpData.Id].Add(wpData);
        }

        Stat = ParseToDict<ItemID, StatData>("Assets/Resources/4_DataTable/Stat.csv", data => data.Id);
        Material = ParseToDict<MaterialID, MaterialData>("Assets/Resources/4_DataTable/Material.csv", data => data.Id);
        Exp = ParseToList<ExpData>("Assets/Resources/4_DataTable/Exp.csv");
        SelectIdol = ParseToList<SelectIdolData>("Assets/Resources/4_DataTable/SelectIdol.csv");
        SelectStage = ParseToList<SelectStageData>("Assets/Resources/4_DataTable/SelectStage.csv");
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