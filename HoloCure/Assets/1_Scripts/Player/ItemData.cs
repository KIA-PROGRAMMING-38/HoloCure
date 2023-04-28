using UnityEngine;

public class ItemData
{
    public int DataKind;
    public int ID;
    public string Name;
    public string DisplayName;
    public string IconSpriteName;
    public int Weight;
    public string Description;
    public Sprite Icon;
}
public enum ItemDataKindID
{
    None = 0,
    Weapon = 1,
    Equipment = 2,
    Stat = 3
}