using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    public static DataManager Data { get; private set; }
    public static ResourceManager Resource { get; private set; }
    public static DataTableManager DataTableM { get; private set; }
    public static PlayerManager PlayerM { get; private set; }
    public static EnemyManager EnemyM { get; private set; }
    public static PresenterManager PresenterM { get; private set; }
    public static ObjectManager ObjectM { get; private set; }
    public static StageManager StageM { get; private set; }
    public static ItemManager ItemM { get; private set; }

    private void Awake()
    {
        Init();

        Time.timeScale = 0;
    }
    private void Init()
    {
        Instance = this;

        DontDestroyOnLoad(this);

        Data = new DataManager();
        Resource = new ResourceManager();

        Data.Init();
        Resource.Init();

        GameObject go;

        go = new GameObject(nameof(DataTableM));
        go.transform.parent = transform;
        DataTableM = go.AddComponent<DataTableManager>();

        go = new GameObject(nameof(PresenterM));
        go.transform.parent = transform;
        PresenterM = go.AddComponent<PresenterManager>();

        go = new GameObject(nameof(PlayerM));
        go.transform.parent = transform;
        PlayerM = go.AddComponent<PlayerManager>();

        go = new GameObject(nameof(EnemyM));
        go.transform.parent = transform;
        EnemyM = go.AddComponent<EnemyManager>();

        go = new GameObject(nameof(ObjectM));
        go.transform.parent = transform;
        ObjectM = go.AddComponent<ObjectManager>();

        go = new GameObject(nameof(StageM));
        go.transform.parent = transform;
        StageM = go.AddComponent<StageManager>();

        go = new GameObject(nameof(ItemM));
        go.transform.parent = transform;
        ItemM = go.AddComponent<ItemManager>();
    }
}
