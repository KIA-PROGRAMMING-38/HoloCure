public class RedHeart : RevolvingWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        if (strikeIndex == 0)
        {
            Managers.Sound.Play(SoundID.RedHeart);
        }
    }

    protected override void SetupStrikeOnOperate(WeaponStrike strike)
    {
        strike.transform.rotation = default;
    }
}