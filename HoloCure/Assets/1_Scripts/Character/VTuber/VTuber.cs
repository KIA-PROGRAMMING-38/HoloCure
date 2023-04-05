using StringLiterals;
using Unity.VisualScripting;
using UnityEngine;

public class VTuber : CharacterBase
{
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    VTuberAnimation _VTuberAnimation;

    private VTuberFeature _VTuberFeature;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
    }
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input.MoveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
    public void InitializePrefab(CharacterStat stat,VTuberFeature feature, VTuberRender render)
    {
        _VTuberAnimation = transform.Find(GameObjectLiteral.BODY).GetComponent<VTuberAnimation>();
        _VTuberAnimation.SetVTuberRender(render);
        
        baseStat = stat;
        _VTuberFeature = feature;

        gameObject.SetActive(false);
    }

    public void IsSelected()
    {
        _input = transform.AddComponent<PlayerInput>();
        transform.AddComponent<PlayerController>();
        _VTuberAnimation.SetInputRef();

        gameObject.SetActive(true);
    }
}
