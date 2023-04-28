using UnityEngine;

public class DataTableManager : MonoBehaviour
{
    public VTuberDataTable VTuberDataTable { get; private set; }
    public EnemyDataTable EnemyDataTable { get; private set; }
    public WeaponDataTable WeaponDataTable { get; private set; }
    public StatDataTable StatDataTable { get; private set; }
    private void Awake()
    {
        VTuberDataTable = new VTuberDataTable();
        EnemyDataTable = new EnemyDataTable();
        WeaponDataTable = new WeaponDataTable();
        StatDataTable = new StatDataTable();
        SetTable();
    }
    public void SetTable()
    {
        VTuberDataTable.SetDataTable();
        EnemyDataTable .SetDataTable();
        WeaponDataTable .SetDataTable();
        StatDataTable.SetDataTable();
    }
}