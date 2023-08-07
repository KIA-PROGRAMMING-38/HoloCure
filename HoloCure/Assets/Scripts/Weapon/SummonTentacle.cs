public class SummonTentacle : CursorTargetingMeleeWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        Managers.Sound.Play(SoundID.SummonTentacle);
    }
}