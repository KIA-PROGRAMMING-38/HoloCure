using UnityEngine;

public class BirdFeather : CursorTargetingRangedWeapon
{
    private const float ANGLE_BETWEEN_STRIKES = 5f;
    private void Awake() => angleBetweenStrikes = ANGLE_BETWEEN_STRIKES;
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        strike.transform.rotation = centerStrikeRotation * Quaternion.AngleAxis(angles[strikeIndex], Vector3.back);

        Managers.Sound.Play(SoundID.BirdFeather);
    }
}