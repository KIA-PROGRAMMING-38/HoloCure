using UnityEngine;

public class KapuKapu : CursorTargetingMeleeWeapon
{
    private const float NEXT_STRIKE_SCALE_MULTIPLIER = 1.5f;
    private Quaternion _firstStrikeRotation;
    protected override void SetupStrikeOnPerform(WeaponStrike strike, int strikeIndex)
    {
        if (strikeIndex == 0)
        {
            _firstStrikeRotation = strike.transform.rotation;
        }
        else
        {
            Vector2 cursorDirectedPosition = (Vector2)strike.transform.position + cursorDirectedOffset;
            strike.transform.position = cursorDirectedPosition;
            strike.transform.rotation = _firstStrikeRotation;
            strike.transform.localScale *= NEXT_STRIKE_SCALE_MULTIPLIER;
        }

        Managers.Sound.Play(SoundID.KapuKapu);
    }
}