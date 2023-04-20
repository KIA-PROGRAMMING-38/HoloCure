using StringLiterals;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponDataTable
{
    private Dictionary<int, WeaponData> _weaponDataContainer = new Dictionary<int, WeaponData>();
    private Dictionary<int, WeaponStat> _weaponStatContainer = new Dictionary<int, WeaponStat>();
    private Dictionary<int, Weapon> _weaponPrefabContainer = new Dictionary<int, Weapon>();

    public Dictionary<int, WeaponData> WeaponDataContainer => _weaponDataContainer;
    public Dictionary<int, WeaponStat> WeaponStatContainer => _weaponStatContainer;
    public Dictionary<int, Weapon> WeaponPrefabContainer => _weaponPrefabContainer;

    public WeaponDataTable()
    {
        SetCommonWeaponData();
        SetStartingWeaponData();
    }
    private void SetCommonWeaponData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.COMMON_WEAPON));
        string[] rows = csvFile.text.Split('\n');

        for (int i = 1; i < rows.Length; i += 7)
        {
            string[] columns = rows[i].Split('|');
            WeaponData data = new WeaponData();
            WeaponStat stat = new WeaponStat();

            data.ID = int.Parse(columns[0]);
            data.Name = columns[1];
            data.DisplayName = columns[2];
            data.DisplaySpriteName = columns[3];
            data.IconSpriteName = columns[4];
            data.Type = columns[5];
            data.Weight = int.Parse(columns[6]);
            data.Display = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.DisplaySpriteName));
            data.Icon = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.IconSpriteName));
            data.ProjectileClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._PROJECTILE));
            data.EffectClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._EFFECT));

            for (int j = 1; j <= 7; ++j)
            {
                columns = rows[i + j - 1].Split('|');

                data.Description[j] = columns[7];
                stat.BaseAttackSequenceTime[j] = float.Parse(columns[8]);
                stat.MinAttackSequenceTime[j] = float.Parse(columns[9]);
                stat.ProjectileCount[j] = int.Parse(columns[10]);
                stat.DamageRate[j] = float.Parse(columns[11]);
                stat.AttackDelay[j] = float.Parse(columns[12]);
                stat.HitCooltime[j] = float.Parse(columns[13]);
                stat.Size[j] = float.Parse(columns[14]);
                stat.AttackDurationTime[j] = float.Parse(columns[15]);
                stat.ProjectileSpeed[j] = int.Parse(columns[16]);
                stat.KnockbackDurationTime[j] = float.Parse(columns[17]);
                stat.KnockbackSpeed[j] = float.Parse(columns[18]);
            }

            _weaponDataContainer.Add(data.ID, data);
            _weaponStatContainer.Add(data.ID, stat);

            Weapon prefab = Resources.Load<Weapon>(Path.Combine(PathLiteral.PREFAB, data.Name));
            _weaponPrefabContainer.Add(data.ID, prefab);
        }
    }

    private void SetStartingWeaponData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(Path.Combine(PathLiteral.DATA_TABLE, FileNameLiteral.STARTING_WEAPON));
        string[] rows = csvFile.text.Split('\n');

        for (int i = 1; i < rows.Length; i += 7)
        {
            string[] columns = rows[i].Split('|');
            WeaponData data = new WeaponData();
            WeaponStat stat = new WeaponStat();

            data.ID = int.Parse(columns[0]);
            data.Name = columns[1];
            data.DisplayName = columns[2];
            data.DisplaySpriteName = columns[3];
            data.IconSpriteName = columns[4];
            data.Type = columns[5];
            data.Weight = int.Parse(columns[6]);
            data.Display = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.DisplaySpriteName));
            data.Icon = Resources.Load<Sprite>(Path.Combine(PathLiteral.SPRITE, PathLiteral.WEAPON, data.IconSpriteName));
            data.ProjectileClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._PROJECTILE));
            data.EffectClip = Resources.Load<AnimationClip>(Path.Combine(PathLiteral.ANIM, PathLiteral.WEAPON, data.Name + FileNameLiteral._EFFECT));

            for (int j = 1; j <= 7; ++j)
            {
                columns = rows[i + j - 1].Split('|');

                data.Description[j] = columns[7];
                stat.BaseAttackSequenceTime[j] = float.Parse(columns[8]);
                stat.MinAttackSequenceTime[j] = float.Parse(columns[9]);
                stat.ProjectileCount[j] = int.Parse(columns[10]);
                stat.DamageRate[j] = float.Parse(columns[11]);
                stat.AttackDelay[j] = float.Parse(columns[12]);
                stat.HitCooltime[j] = float.Parse(columns[13]);
                stat.Size[j] = float.Parse(columns[14]);
                stat.AttackDurationTime[j] = float.Parse(columns[15]);
                stat.ProjectileSpeed[j] = int.Parse(columns[16]);
                stat.KnockbackDurationTime[j] = float.Parse(columns[17]);
                stat.KnockbackSpeed[j] = float.Parse(columns[18]);
            }

            _weaponDataContainer.Add(data.ID, data);
            _weaponStatContainer.Add(data.ID, stat);

            Weapon prefab = Resources.Load<Weapon>(Path.Combine(PathLiteral.PREFAB, data.Name));
            _weaponPrefabContainer.Add(data.ID, prefab);
        }
    }
}