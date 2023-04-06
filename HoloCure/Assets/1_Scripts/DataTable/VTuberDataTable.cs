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

    private Dictionary<VTuberID, VTuberRender> _VTuberRenderContainer = new Dictionary<VTuberID, VTuberRender>();

    public Dictionary<VTuberID, VTuberData> VTuberDataContainer => _VTuberDataContainer;
    public Dictionary<VTuberID, CharacterStat> VTuberStatContainer => _VTuberStatContainer;
    public Dictionary<VTuberID, VTuberFeature> VTuberFeatureContainer => _VTuberFeatureContainer;
    public Dictionary<VTuberID, VTuber> VTuberPrefabContainer => _VTuberPrefabContainer;
    public Dictionary<VTuberID, VTuberRender> VTuberRenderContainer => _VTuberRenderContainer;

    public VTuberDataTable()
    {
        SetVTuberData();
    }
    private void SetVTuberData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.VTUBER));
        string[] rows = csvFile.text.Split('\n');

        VTuber defaultPrefab = Resources.Load<VTuber>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.VTUBER)).GetComponent<VTuber>();
        for (int i = 1; i < rows.Length; ++i)
        {
            string[] columns = rows[i].Split(',');
            VTuberData data = new VTuberData();
            CharacterStat stat = new CharacterStat();
            VTuberFeature feature = new VTuberFeature();
            VTuberRender render = new VTuberRender();

            data.ID = (VTuberID)int.Parse(columns[0]);
            data.Name = columns[1];
            data.SpriteName = columns[2];
            data.PortraitName = columns[3];

            stat.MaxHealth = int.Parse(columns[4]);
            stat.ATKPower = float.Parse(columns[5]);
            stat.MoveSpeedRate = float.Parse(columns[6]);
            feature.CrticalRate = int.Parse(columns[7]);
            feature.PickupSize = int.Parse(columns[8]);
            feature.Haste = int.Parse(columns[9]);

            _VTuberDataContainer.Add(data.ID, data);
            _VTuberStatContainer.Add(data.ID, stat);
            _VTuberFeatureContainer.Add(data.ID, feature);

            render.Sprite = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, data.SpriteName));
            render.Portrait = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, data.PortraitName));
            render.IdleClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, FileNameLiteral.IDLE));
            render.RunClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.VTUBER, data.Name, FileNameLiteral.RUN));

            _VTuberRenderContainer.Add(data.ID, render);

            VTuber prefab = Object.Instantiate(defaultPrefab);
            
            prefab.Initialize(stat, feature, render);

            _VTuberPrefabContainer.Add(data.ID, prefab);
        }
    }
}