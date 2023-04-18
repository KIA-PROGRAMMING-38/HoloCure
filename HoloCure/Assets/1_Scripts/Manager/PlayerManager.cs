using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameManager _gameManager;
    private DataTableManager _dataTableManager;
    private VTuberDataTable _VTuberDataTable;
    private PresenterManager _presenterManager;

    public GameManager GameManager
    {
        private get => _gameManager;
        set
        {
            _gameManager = value;
            _dataTableManager = _gameManager.DataTableManager;
            _VTuberDataTable = _dataTableManager.VTuberDataTable;
            _presenterManager = _gameManager.PresenterManager;

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SelectVTuber(VTuberID.Ninomae_Inanis);
        }
    }
    public Player Player { get; private set; }
    public VTuber VTuber { get; private set; }

    // 캐릭터 선택창과 연동해야 하는부분
    private void SelectVTuber(VTuberID ID)
    {
        VTuber = _VTuberDataTable.VTuberPrefabContainer[ID];
        VTuber.IsSelected(ID, _dataTableManager.WeaponDataTable);
        Player = VTuber.GetComponent<Player>();

        _presenterManager.InitPresenter.GetInitData(_VTuberDataTable.VTuberDataContainer[ID]);

        Player.OnGetExp -= _presenterManager.ExpPresenter.UpdateExpGauge;
        Player.OnGetExp += _presenterManager.ExpPresenter.UpdateExpGauge;

        Player.OnLevelUp -= _presenterManager.CountPresenter.UpdatePlayerLevelCount;
        Player.OnLevelUp += _presenterManager.CountPresenter.UpdatePlayerLevelCount;

        VTuber.OnChangeMaxHp -= _presenterManager.HPPresenter.UpdateMaxHp;
        VTuber.OnChangeMaxHp += _presenterManager.HPPresenter.UpdateMaxHp;

        VTuber.OnGetDamage -= _presenterManager.HPPresenter.UpdateCurHp;
        VTuber.OnGetDamage += _presenterManager.HPPresenter.UpdateCurHp;

        VTuber.OnChangeATKRate -= _presenterManager.StatPresenter.UpdateATK;
        VTuber.OnChangeATKRate += _presenterManager.StatPresenter.UpdateATK;

        VTuber.OnChangeATKRate -= _presenterManager.StatPresenter.UpdateATK;
        VTuber.OnChangeATKRate += _presenterManager.StatPresenter.UpdateATK;

        VTuber.OnChangeSPDRate -= _presenterManager.StatPresenter.UpdateSPD;
        VTuber.OnChangeSPDRate += _presenterManager.StatPresenter.UpdateSPD;

        VTuber.OnChangeCRTRate -= _presenterManager.StatPresenter.UpdateCRT;
        VTuber.OnChangeCRTRate += _presenterManager.StatPresenter.UpdateCRT;

        VTuber.OnChangePickupRate -= _presenterManager.StatPresenter.UpdatePickup;
        VTuber.OnChangePickupRate += _presenterManager.StatPresenter.UpdatePickup;

        VTuber.OnChangeHasteRate -= _presenterManager.StatPresenter.UpdateHaste;
        VTuber.OnChangeHasteRate += _presenterManager.StatPresenter.UpdateHaste;

        Player.InitializeEvent();
        VTuber.InitializeEvent();
    }

    private void Start()
    {

    }
}