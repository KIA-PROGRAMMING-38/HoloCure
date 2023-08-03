public class ScytheSwing : CursorTargetingMeleeWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        Managers.Sound.Play(SoundID.ScytheSwing);
    }
}