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
    public int Attack { get; set; }
    public int Speed { get; set; }
    public int Critical { get; set; }
    public int PickUp { get; set; }
    public int Haste { get; set; }
}
