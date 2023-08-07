public class Orbit : RevolvingWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        Managers.Sound.Play(SoundID.Orbit);
    }
}