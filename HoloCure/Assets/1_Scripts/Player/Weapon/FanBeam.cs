using UnityEngine;

public class FanBeam : Weapon
{
    private PlayerInput _input;
    protected override void Awake()
    {
        base.Awake();
        _input = VTuber.GetComponent<PlayerInput>();
    }
    protected override void Operate()
    {
        
    }
    protected override void BeforeOperate()
    {
        Vector2 direction = _input.MouseWorldPos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        projectiles[0].transform.position = (Vector2)transform.position + direction.normalized * 20;
        projectiles[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}