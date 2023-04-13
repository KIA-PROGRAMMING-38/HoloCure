﻿using UnityEngine;

public class VTuberManager : MonoBehaviour
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
        }
    }

    public VTuber VTuber { get;private set; }
    private void Start()
    {
        // 캐릭터 선택창과 연동해야 하는부분
        VTuber = _VTuberDataTable.VTuberPrefabContainer[VTuberID.Ninomae_Inanis];
        VTuber.IsSelected(VTuberID.Ninomae_Inanis, _dataTableManager.WeaponDataTable);
    }
}