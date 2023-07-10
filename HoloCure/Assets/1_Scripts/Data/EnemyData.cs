public enum EnemyID
{
    None = 10000,
    Stage01_Normal = 11100,
    Stage01_Shrimp,
    Stage01_Deadbeat,
    Stage01_Takodachi,
    Stage01_KFP,
    Stage01_ShrimpDark,
    Stage01_Gloom,
    Stage01_Bloom,
    Stage01_DeadbeatBatter,
    Stage01_InvestiGator,
    Stage01_TakodachiAngry,
    Stage01_KFPAngry,
    Stage01_Baerat,
    Stage01_KromieA,
    Stage01_KromieB,
    Stage01_ShrimpGangA,
    Stage01_DeadbeatGangA,
    Stage01_SaplingA,
    Stage01_SaplingB,
    Stage01_SaplingC,
    Stage01_HoomanA,
    Stage01_HoomanB,
    Stage01_BreadDog,
    Stage01_SaplingKing,
    Stage01_KromieKing,
    Stage01_Bubba,
    Stage01_MiniBoss = 11200,
    Stage01_MiniBoss_Shrimp,
    Stage01_MiniBoss_Takodachi,
    Stage01_MiniBoss_ShrimpDark,
    Stage01_MiniBoss_DeadbeatBatter,
    Stage01_MiniBoss_KromieKing,
    Stage01_MiniBoss_ShrimpGangA,
    Stage01_MiniBoss_DeadbeatGangA,
    Stage01_MiniBoss_SaplingKing,
    Stage01_Boss = 11300,
    Stage01_Boss_Fubuzilla,
    Stage01_Boss_SmolAme,
}
public class EnemyData
{
    public EnemyID Id { get; set; }
    public string Name { get; set; }
    public string Sprite { get; set; }
    public int Health { get; set; }
    public int ATK { get; set; }
    public float SPD { get; set; }
    public int Exp { get; set; }
    public float Scale { get; set; }
    public int SpawnStartTime { get; set; }
    public int SpawnEndTime { get; set; }
}
