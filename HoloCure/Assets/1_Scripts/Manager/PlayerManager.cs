using StringLiterals;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public VTuber VTuber { get; private set; }

    private void Start()
    {
        Managers.PresenterM.TitleUIPresenter.OnPlayGameForPlayer -= SelectVTuber;
        Managers.PresenterM.TitleUIPresenter.OnPlayGameForPlayer += SelectVTuber;

        Managers.PresenterM.TriggerUIPresenter.OnGameEnd -= GameEnd;
        Managers.PresenterM.TriggerUIPresenter.OnGameEnd += GameEnd;
    }
    private void GameEnd()
    {
        Managers.PresenterM.TriggerUIPresenter.OnSendSelectedID -= Player.Inventory.GetItem;
        Destroy(Player.Inventory.gameObject);
        Destroy(Player.GetComponent<PlayerInput>());
        Destroy(Player.GetComponent<PlayerController>());
        Destroy(Player.GetComponent<Player>());
        VTuber.gameObject.SetActive(false);
    }

    private void SelectVTuber(VTuberID id)
    {
        VTuber = Managers.Resource.Instantiate(FileNameLiteral.VTUBER).GetComponent<VTuber>();

        VTuber.Init(id);
        Player = VTuber.GetComponent<Player>();

        VTuberData data = Managers.Data.VTuber[id];

        Managers.PresenterM.InitPresenter.GetInitData(data);

        Player.OnGetExp -= Managers.PresenterM.ExpPresenter.UpdateExpGauge;
        Player.OnGetExp += Managers.PresenterM.ExpPresenter.UpdateExpGauge;

        Player.OnGetBox -= Managers.PresenterM.TriggerUIPresenter.ActivateGetBoxStartUI;
        Player.OnGetBox += Managers.PresenterM.TriggerUIPresenter.ActivateGetBoxStartUI;

        Player.OnLevelUp -= Managers.PresenterM.CountPresenter.UpdatePlayerLevelCount;
        Player.OnLevelUp += Managers.PresenterM.CountPresenter.UpdatePlayerLevelCount;
        Player.OnLevelUp -= Managers.PresenterM.TriggerUIPresenter.ActivateLevelUpUI;
        Player.OnLevelUp += Managers.PresenterM.TriggerUIPresenter.ActivateLevelUpUI;

        Managers.PresenterM.TriggerUIPresenter.OnSendSelectedID -= Player.Inventory.GetItem;
        Managers.PresenterM.TriggerUIPresenter.OnSendSelectedID += Player.Inventory.GetItem;

        Player.Inventory.OnNewEquipmentEquip -= Managers.PresenterM.InventoryPresenter.UpdateNewEquipment;
        Player.Inventory.OnNewEquipmentEquip += Managers.PresenterM.InventoryPresenter.UpdateNewEquipment;

        Player.Inventory.OnEquipmentLevelUp -= Managers.PresenterM.InventoryPresenter.UpdateEquipmentLevel;
        Player.Inventory.OnEquipmentLevelUp += Managers.PresenterM.InventoryPresenter.UpdateEquipmentLevel;

        VTuber.OnChangeMaxHp -= Managers.PresenterM.HPPresenter.UpdateMaxHp;
        VTuber.OnChangeMaxHp += Managers.PresenterM.HPPresenter.UpdateMaxHp;

        VTuber.OnChangeCurHP -= Managers.PresenterM.HPPresenter.UpdateCurHp;
        VTuber.OnChangeCurHP += Managers.PresenterM.HPPresenter.UpdateCurHp;

        VTuber.OnChangeATKRate -= Managers.PresenterM.StatPresenter.UpdateATK;
        VTuber.OnChangeATKRate += Managers.PresenterM.StatPresenter.UpdateATK;

        VTuber.OnChangeATKRate -= Managers.PresenterM.StatPresenter.UpdateATK;
        VTuber.OnChangeATKRate += Managers.PresenterM.StatPresenter.UpdateATK;

        VTuber.OnChangeSPDRate -= Managers.PresenterM.StatPresenter.UpdateSPD;
        VTuber.OnChangeSPDRate += Managers.PresenterM.StatPresenter.UpdateSPD;

        VTuber.OnChangeCRTRate -= Managers.PresenterM.StatPresenter.UpdateCRT;
        VTuber.OnChangeCRTRate += Managers.PresenterM.StatPresenter.UpdateCRT;

        VTuber.OnChangePickupRate -= Managers.PresenterM.StatPresenter.UpdatePickup;
        VTuber.OnChangePickupRate += Managers.PresenterM.StatPresenter.UpdatePickup;

        VTuber.OnChangeHasteRate -= Managers.PresenterM.StatPresenter.UpdateHaste;
        VTuber.OnChangeHasteRate += Managers.PresenterM.StatPresenter.UpdateHaste;

        VTuber.OnDie -= Managers.PresenterM.TriggerUIPresenter.ActivateGameOverUI;
        VTuber.OnDie += Managers.PresenterM.TriggerUIPresenter.ActivateGameOverUI;

        Player.InitializeEvent();
        VTuber.InitializeEvent();
        VTuber.transform.position = default;
        Managers.PresenterM.InventoryPresenter.ResetInventory();
        Managers.PresenterM.CountPresenter.ResetCount();
        Player.Inventory.GetItem(data.StartingWeaponId);
    }
}