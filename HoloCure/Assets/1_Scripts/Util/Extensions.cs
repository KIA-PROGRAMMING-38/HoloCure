using UnityEngine;

public static class Extensions
{
    public static T GetRandomElement<T>(this T[] array) => array[Random.Range(0, array.Length)];
    public static T GetRandomElement<T>(this T[] array, int start, int end) => array[Random.Range(start, end)];
    public static EnemyType GetEnemyType(this EnemyID id, int stage)
    {
        id -= stage * 1000;

        if (EnemyID.Normal < id && id < EnemyID.MiniBoss) { return EnemyType.Normal; }
        if (EnemyID.MiniBoss < id && id < EnemyID.Boss) { return EnemyType.MiniBoss; }
        if (EnemyID.Boss < id && id < EnemyID.End) { return EnemyType.Boss; }
        return EnemyType.None;
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
        if(ItemID.CommonWeapon < id && id < ItemID.StartingWeapon) { return ItemType.CommonWeapon; }
        if(ItemID.StartingWeapon < id && id < ItemID.Equipment) { return ItemType.StartingWeapon; }
        if(ItemID.Equipment < id && id < ItemID.Stat) { return ItemType.Equipment; }
        if(ItemID.Stat < id && id < ItemID.End) { return ItemType.Stat; }
        return ItemType.None;
    }
}