using UnityEngine;

public class Fubuzilla : Enemy
{
    [SerializeField] private FubuzillaAttackPointer _attackPointer;
    private Vector3 _initScale;
    private Vector3 _filpScale;
    [SerializeField]private bool _isShoot;
    protected override void Awake()
    {
        base.Awake();

        _initScale = _attackPointer.transform.localScale;
        _filpScale = _initScale;
        _filpScale.x *= -1;

        _attackPointer.OnReady -= SetIsShootFalse;
        _attackPointer.OnReady += SetIsShootFalse;
        _attackPointer.OnShoot -= SetIsShootTrue;
        _attackPointer.OnShoot += SetIsShootTrue;
    }

    private void SetIsShootTrue() => _isShoot = true;
    private void SetIsShootFalse()
    {
        _isShoot = false;

        FilpAttackPointer();
    }

    private void FilpAttackPointer()
    {
        if (_isShoot)
        {
            return;
        }

        if (enemyAnimation.IsFlip)
        {
            _attackPointer.transform.localScale = _filpScale;
        }
        else
        {
            _attackPointer.transform.localScale = _initScale;
        }
    }
}
