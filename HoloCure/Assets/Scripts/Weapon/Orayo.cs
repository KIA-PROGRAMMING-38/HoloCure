using UnityEngine;

public class Orayo : CursorTargetingRangedWeapon
{
    private const float ANGLE_BETWEEN_STRIKES = 15f;
    private void Awake() => angleBetweenStrikes = ANGLE_BETWEEN_STRIKES;
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation * Quaternion.AngleAxis(angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.Orayo);
    }
}