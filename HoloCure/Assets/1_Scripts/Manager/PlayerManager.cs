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
    public Player Player { get; private set; }
    public VTuber VTuber { get; private set; }

    private void Start()
    {
        _presenterManager.TitleUIPresenter.OnPlayGameForPlayer -= SelectVTuber;
        _presenterManager.TitleUIPresenter.OnPlayGameForPlayer += SelectVTuber;

        _presenterManager.TriggerUIPresenter.OnGameEnd -= GameEnd;
        _presenterManager.TriggerUIPresenter.OnGameEnd += GameEnd;
    }
    private void GameEnd()
    {
        _presenterManager.TriggerUIPresenter.OnSendSelectedID -= Player.Inventory.GetItem;
        Destroy(Player.Inventory.gameObject);
        Destroy(Player.GetComponent<PlayerInput>());
        Destroy(Player.GetComponent<PlayerController>());
        Destroy(Player.GetComponent<Player>());
        VTuber.gameObject.SetActive(false);
    }

    private void SelectVTuber(VTuberID ID)
    {
        VTuber = _VTuberDataTable.VTuberPrefabContainer[ID];
        VTuber.IsSelected(ID, _dataTableManager.WeaponDataTable, _dataTableManager.StatDataTable);
        Player = VTuber.GetComponent<Player>();

        _presenterManager.InitPresenter.GetInitData(_VTuberDataTable.VTuberDataContainer[ID]);

        Player.OnGetExp -= _presenterManager.ExpPresenter.UpdateExpGauge;
        Player.OnGetExp += _presenterManager.ExpPresenter.UpdateExpGauge;

        Player.OnGetBox -= _presenterManager.TriggerUIPresenter.ActivateGetBoxStartUI;
        Player.OnGetBox += _presenterManager.TriggerUIPresenter.ActivateGetBoxStartUI;

        Player.OnLevelUp -= _presenterManager.CountPresenter.UpdatePlayerLevelCount;
        Player.OnLevelUp += _presenterManager.CountPresenter.UpdatePlayerLevelCount;
        Player.OnLevelUp -= _presenterManager.TriggerUIPresenter.ActivateLevelUpUI;
        Player.OnLevelUp += _presenterManager.TriggerUIPresenter.ActivateLevelUpUI;

        _presenterManager.TriggerUIPresenter.OnSendSelectedID -= Player.Inventory.GetItem;
        _presenterManager.TriggerUIPresenter.OnSendSelectedID += Player.Inventory.GetItem;

        Player.Inventory.OnNewEquipmentEquip -= _presenterManager.InventoryPresenter.UpdateNewEquipment;
        Player.Inventory.OnNewEquipmentEquip += _presenterManager.InventoryPresenter.UpdateNewEquipment;

        Player.Inventory.OnEquipmentLevelUp -= _presenterManager.InventoryPresenter.UpdateEquipmentLevel;
        Player.Inventory.OnEquipmentLevelUp += _presenterManager.InventoryPresenter.UpdateEquipmentLevel;

        VTuber.OnChangeMaxHp -= _presenterManager.HPPresenter.UpdateMaxHp;
        VTuber.OnChangeMaxHp += _presenterManager.HPPresenter.UpdateMaxHp;

        VTuber.OnChangeCurHP -= _presenterManager.HPPresenter.UpdateCurHp;
        VTuber.OnChangeCurHP += _presenterManager.HPPresenter.UpdateCurHp;

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
        VTuber.transform.position = default;
        _presenterManager.InventoryPresenter.ResetInventory();
        _presenterManager.CountPresenter.ResetCount();
        Player.Inventory.GetItem((int)ID - 4000);
    }
}