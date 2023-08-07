public class PhoenixSword : CursorTargetingMeleeWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        Managers.Sound.Play(SoundID.PhoenixSword);
    }
}