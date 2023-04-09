using UnityEngine;

public class DataTableManager : MonoBehaviour
{
    public VTuberDataTable VTuberDataTable { get; private set; }
    public EnemyDataTable EnemyDataTable { get; private set; }
    public WeaponDataTable WeaponDataTable { get; private set; }
    private void Awake()
    {
        VTuberDataTable = new VTuberDataTable();
        EnemyDataTable = new EnemyDataTable();
        WeaponDataTable = new WeaponDataTable();
    }
}