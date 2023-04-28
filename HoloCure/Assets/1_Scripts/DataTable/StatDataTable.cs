using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatDataTable
{
    private Dictionary<int, Stat> _statContainer = new();
    public Dictionary<int, Stat> StatContainer => _statContainer;
    public void SetDataTable()
    {
        SetStatData();
    }
    private void SetStatData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.STAT));
        string[] rows = csvFile.text.Split('\n');

        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split('|');
            Stat stat = new();

            stat.DataKind = 3;

            stat.ID = int.Parse(columns[0]);
            stat.Name = columns[1];
            stat.DisplayName = columns[2];
            stat.IconSpriteName = columns[3];
            stat.Weight = int.Parse(columns[4]);
            stat.Description = columns[5];
            stat.Value = int.Parse(columns[6]);
            stat.Icon = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.UI, stat.IconSpriteName));

            _statContainer.Add(stat.ID, stat);
        }
    }
}