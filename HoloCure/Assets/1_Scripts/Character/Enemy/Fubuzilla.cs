using UnityEngine;

public class Fubuzilla : Boss
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

        OnFilp -= FilpAttackPointer;
        OnFilp += FilpAttackPointer;

        _attackPointer.OnReady -= SetIsShootFalse;
        _attackPointer.OnReady += SetIsShootFalse;
        _attackPointer.OnShoot -= SetIsShootTrue;
        _attackPointer.OnShoot += SetIsShootTrue;
    }

    private void SetIsShootTrue() => _isShoot = true;
    private void SetIsShootFalse()
    {
        _isShoot = false;

        FilpAttackPointer(enemyAnimation.IsFilp());
    }

    private void FilpAttackPointer(bool isFilp)
    {
        if (_isShoot)
        {
            return;
        }

        if (isFilp)
        {
            _attackPointer.transform.localScale = _filpScale;
        }
        else
        {
            _attackPointer.transform.localScale = _initScale;
        }
    }
}
