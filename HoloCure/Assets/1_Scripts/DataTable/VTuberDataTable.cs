using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VTuberDataTable
{
    private Dictionary<VTuberID, VTuberData> _VTuberDataContainer = new Dictionary<VTuberID, VTuberData>();
    private Dictionary<VTuberID, CharacterStat> _VTuberStatContainer = new Dictionary<VTuberID, CharacterStat>();
    private Dictionary<VTuberID, VTuberFeature> _VTuberFeatureContainer = new Dictionary<VTuberID, VTuberFeature>();

    private Dictionary<VTuberID, VTuber> _VTuberPrefabContainer = new Dictionary<VTuberID, VTuber>();

    public Dictionary<VTuberID, VTuberData> VTuberDataContainer => _VTuberDataContainer;
    public Dictionary<VTuberID, CharacterStat> VTuberStatContainer => _VTuberStatContainer;
    public Dictionary<VTuberID, VTuberFeature> VTuberFeatureContainer => _VTuberFeatureContainer;
    public Dictionary<VTuberID, VTuber> VTuberPrefabContainer => _VTuberPrefabContainer;
    public void SetDataTable()
    {
        SetVTuberData();
    }
    private void SetVTuberData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.VTUBER));
        string[] rows = csvFile.text.Split('\n');

        VTuber defaultPrefab = Resources.Load<VTuber>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.VTUBER));
        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split(',');
            VTuberData data = new VTuberData();
            CharacterStat stat = new CharacterStat();
            VTuberFeature feature = new VTuberFeature();

            data.ID = (VTuberID)int.Parse(columns[0]);
            data.Name = columns[1];
            data.DisplayName = columns[2];
            data.DisplaySpriteName = columns[3];
            data.PortraitSpriteName = columns[4];
            data.TitleSpriteName = columns[5];

            data.Display = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, data.DisplaySpriteName));
            data.Portrait = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, data.PortraitSpriteName));
            data.Title = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, data.TitleSpriteName));

            data.IdleClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, FileNameLiteral.IDLE));
            data.RunClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, FileNameLiteral.RUN));

            stat.MaxHealth = int.Parse(columns[6]);
            stat.ATKPower = float.Parse(columns[7]);
            stat.MoveSpeedRate = float.Parse(columns[8]);
            feature.CRTRate = int.Parse(columns[9]);
            feature.PickupRate = int.Parse(columns[10]);
            feature.HasteRate = int.Parse(columns[11]);

            _VTuberDataContainer.Add(data.ID, data);
            _VTuberStatContainer.Add(data.ID, stat);
            _VTuberFeatureContainer.Add(data.ID, feature);

            VTuber prefab = Object.Instantiate(defaultPrefab);
            
            prefab.Initialize(stat, feature, data);

            _VTuberPrefabContainer.Add(data.ID, prefab);
        }
    }
}