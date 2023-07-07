using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

public class VTuberData2
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplaySprite { get; set; }
    public string PortraitSprite { get; set; }
    public string TitleSprite { get; set; }
    public float Health { get; set; }
    public float ATK { get; set; }
    public float SPD { get; set; }
    public float CRT { get; set; }
    public float PickUp { get; set; }
    public float Haste { get; set; }
}

public class EnemyData2
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sprite { get; set; }
    public float Health { get; set; }
    public float ATK { get; set; }
    public float SPD { get; set; }
    public int Exp { get; set; }
    public int SpawnStartTime { get; set; }
    public int SpawnEndTime { get; set; }
}

public class DataManager
{
    // NOTE : 데이터테이블 추가해야함.
    // NOTE : 느슨한 식별자의 경우 List를, 엄격한 식별자의 경우 Dictionary 사용.
    // NOTE : Id를 열거형으로 만들어두면 오류낼 일이 적음
    public Dictionary<int, VTuberData2> VTuberDataTable { get; private set; }
    public Dictionary<int, EnemyData2> EnemyDataTable { get; private set; }

    public void Init()
    {
        VTuberDataTable = ParseToDict<int, VTuberData2>("Assets/Resources/4_DataTable/VTuber2.csv", data => data.Id);
        EnemyDataTable = ParseToDict<int, EnemyData2>("Assets/Resources/4_DataTable/Enemy2.csv", data => data.Id);
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