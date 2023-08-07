public class BaseballPitch : CursorTargetingRangedWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        Managers.Sound.Play(SoundID.BaseballPitch);
    }
}