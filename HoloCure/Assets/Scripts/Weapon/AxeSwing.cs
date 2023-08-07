public class AxeSwing : CursorTargetingMeleeWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        Managers.Sound.Play(SoundID.AxeSwing);
    }
}