public enum VTuberID
{
    None = 1000,
    Ina = 1101,
}
public class VTuberData
{
    public VTuberID Id { get; set; }
    public string Name { get; set; }
    public string DisplaySprite { get; set; }
    public string PortraitSprite { get; set; }
    public string TitleSprite { get; set; }
    public ItemID StartingWeaponId { get; set; }
    public int Health { get; set; }
    public float ATK { get; set; }
    public float SPD { get; set; }
    public float CRT { get; set; }
    public float PickUp { get; set; }
    public float Haste { get; set; }
}
