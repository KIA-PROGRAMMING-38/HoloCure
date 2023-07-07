using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    public static DataTableManager DataTableM { get; private set; }
    public static PlayerManager PlayerM { get; private set; }
    public static EnemyManager EnemyM { get; private set; }
    public static PresenterManager PresenterM { get; private set; }
    public static ObjectManager ObjectM { get; private set; }
    public static StageManager StageM { get; private set; }
    public static ItemManager ItemM { get; private set; }

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);
        
        SetupManager();

        Time.timeScale = 0;
    }
    private void SetupManager()
    {
        GameObject gameObject;

        gameObject = new GameObject(nameof(DataTableM));
        gameObject.transform.parent = transform;
        DataTableM = gameObject.AddComponent<DataTableManager>();

        gameObject = new GameObject(nameof(PresenterM));
        gameObject.transform.parent = transform;
        PresenterM = gameObject.AddComponent<PresenterManager>();

        gameObject = new GameObject(nameof(PlayerM));
        gameObject.transform.parent = transform;
        PlayerM = gameObject.AddComponent<PlayerManager>();

        gameObject = new GameObject(nameof(EnemyM));
        gameObject.transform.parent = transform;
        EnemyM = gameObject.AddComponent<EnemyManager>();

        gameObject = new GameObject(nameof(ObjectM));
        gameObject.transform.parent = transform;
        ObjectM = gameObject.AddComponent<ObjectManager>();

        gameObject = new GameObject(nameof(StageM));
        gameObject.transform.parent = transform;
        StageM = gameObject.AddComponent<StageManager>();

        gameObject = new GameObject(nameof(ItemM));
        gameObject.transform.parent = transform;
        ItemM = gameObject.AddComponent<ItemManager>();
    }
}
