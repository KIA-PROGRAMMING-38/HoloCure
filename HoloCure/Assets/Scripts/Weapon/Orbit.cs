public class Orbit : RevolvingWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        if (strikeIndex == 0)
        {
            Managers.Sound.Play(SoundID.Orbit);
        }
    }
}