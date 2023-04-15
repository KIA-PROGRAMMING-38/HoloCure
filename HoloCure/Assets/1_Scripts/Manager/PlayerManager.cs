﻿using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameManager _gameManager;
    private DataTableManager _dataTableManager;
    private VTuberDataTable _VTuberDataTable;

    public GameManager GameManager
    {
        private get => _gameManager;
        set
        {
            _gameManager = value;
            _dataTableManager = _gameManager.DataTableManager;
            _VTuberDataTable = _dataTableManager.VTuberDataTable;

            SelectVTuber(VTuberID.Ninomae_Inanis);
        }
    }
    public Player Player { get; private set; }
    public VTuber VTuber { get;private set; }
    // 캐릭터 선택창과 연동해야 하는부분
    private void SelectVTuber(VTuberID ID)
    {
        VTuber = _VTuberDataTable.VTuberPrefabContainer[ID];
        VTuber.IsSelected(ID, _dataTableManager.WeaponDataTable);
        Player = VTuber.GetComponent<Player>();
    }

    private void Start()
    {
        Player.OnGetExp -= GameManager.PresenterManager.ExpPresenter.UpdateExpGauge;
        Player.OnGetExp += GameManager.PresenterManager.ExpPresenter.UpdateExpGauge;

        Player.OnLevelUp -= GameManager.PresenterManager.PlayerLevelPresenter.UpdatePlayerLevel;
        Player.OnLevelUp += GameManager.PresenterManager.PlayerLevelPresenter.UpdatePlayerLevel;
    }
}   