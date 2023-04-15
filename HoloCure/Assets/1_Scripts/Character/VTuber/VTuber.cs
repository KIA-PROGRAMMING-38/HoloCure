using StringLiterals;
using Unity.VisualScripting;
using UnityEngine;

public class VTuber : CharacterBase
{
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private VTuberAnimation _VTuberAnimation;

    private VTuberFeature _VTuberFeature;

    // 임시 코드
    public float AtkPower { get; private set; }
    public int CriticalRate { get; private set; }


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;

        _VTuberAnimation = transform.Find(GameObjectLiteral.BODY).GetComponent<VTuberAnimation>();
    }
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input.MoveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
    public void Initialize(CharacterStat stat, VTuberFeature feature, VTuberRender render)
    {
        _VTuberAnimation.SetVTuberRender(render);

        baseStat = stat;
        _VTuberFeature = feature;

        // 임시 코드
        AtkPower = stat.ATKPower * 10;
        CriticalRate = feature.CrticalRate;

        gameObject.SetActive(false);
    }

    public void IsSelected(VTuberID VTuberID, WeaponDataTable weaponDataTable)
    {
        transform.AddComponent<Player>().Initialize(this, VTuberID, weaponDataTable);
        _input = transform.AddComponent<PlayerInput>();
        transform.AddComponent<PlayerController>().Initialize(this);

        _VTuberAnimation.SetInputRef();

        gameObject.SetActive(true);
    }
}
