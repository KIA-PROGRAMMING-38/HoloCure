using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static void RotateLookCursor(this Transform transform) 
        => transform.rotation = Quaternion.AngleAxis(CursorCache.GetAngleToCursor(transform.position), Vector3.forward);
    public static Vector2 GetPositionToCursor(this Vector2 position, float distance) 
        => CursorCache.GetDirectionToCursor(position) * distance;
    public static void BindViewEvent(this UIBehaviour view, Action<PointerEventData> action, ViewEvent type, Component component)
        => UIBase.BindViewEvent(view, action, type, component);
    public static void BindModelEvent<T>(this ReactiveProperty<T> model, Action<T> action, Component component)
        => UIBase.BindModelEvent(model, action, component);
    public static T GetRandomElement<T>(this T[] array)
        => array[Random.Range(0, array.Length)];
    public static T GetRandomElement<T>(this T[] array, int start, int end)
        => array[Random.Range(start, end)];

    private const int ENEMY_TYPE_EXTRACT_VALUE = 100;
    public static EnemyType GetEnemyType(this EnemyID id)
    {
        int typeValue = (int)id / ENEMY_TYPE_EXTRACT_VALUE % 10;
        return (EnemyType)typeValue;
    }
    private const int ENEMY_STAGE_EXTRACT_VALUE = 1000;
    public static int GetStage(this EnemyID id)
    {
        return (int)id / ENEMY_STAGE_EXTRACT_VALUE % 10;
    }

    public static ExpType GetExpType(this int expAmount)
    {
        if (expAmount >= (int)ExpType.Max) { return ExpType.Max; }
        if (expAmount >= (int)ExpType.Four) { return ExpType.Four; }
        if (expAmount >= (int)ExpType.Three) { return ExpType.Three; }
        if (expAmount >= (int)ExpType.Two) { return ExpType.Two; }
        if (expAmount >= (int)ExpType.One) { return ExpType.One; }
        return ExpType.Zero;
    }
    public static ItemType GetItemType(this ItemID id)
    {
        if (ItemID.CommonNone < id && id < ItemID.EquipmentNone) { return ItemType.Weapon; }
        if (ItemID.EquipmentNone < id && id < ItemID.StatNone) { return ItemType.Equipment; }
        if (ItemID.StatNone < id && id < ItemID.End) { return ItemType.Stat; }
        return ItemType.None;
    }
    public static WeaponType GetWeaponType(this ItemID id)
    {
        if (ItemID.CommonNone < id && id < ItemID.StartingNone) { return WeaponType.Common; }
        if (ItemID.StartingNone < id && id < ItemID.EquipmentNone) { return WeaponType.Starting; }
        return WeaponType.None;
    }
    public static SoundType GetSoundType(this SoundID id)
    {
        if (SoundID.BGM < id && id < SoundID.Common) { return SoundType.BGM; }
        if (SoundID.Common < id && id < SoundID.Effect) { return SoundType.Common; }
        if (SoundID.Effect < id && id < SoundID.End) { return SoundType.Effect; }
        return SoundType.Max;
    }
}