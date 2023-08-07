public class BLBook : RevolvingWeapon
{
    protected override void SetupStrikeOnOperate(WeaponStrike strike)
    {
        strike.transform.rotation = default;
    }
}