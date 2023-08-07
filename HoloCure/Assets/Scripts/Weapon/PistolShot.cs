public class PistolShot : CursorTargetingRangedWeapon
{
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation;

        Managers.Sound.Play(SoundID.PistolShot);
    }
}