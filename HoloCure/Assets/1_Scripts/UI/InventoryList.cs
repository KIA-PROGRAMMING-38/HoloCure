using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private GameObject _frame;
    [SerializeField] private Image _levelNumImage;

    [SerializeField] private Sprite[] _numsImage;
    private Image _image;
    private Sprite _defaultSprite;
    private readonly Color DefaultColor = new(1, 1, 1, 0.5f);
    private readonly Color EquipColor = new(1, 1, 1, 1);
    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultSprite = _image.sprite;
    }
    public ItemID Id {  get; set; }
    public void UpdateNewEquipment(Sprite sprite)
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
        
        _image.sprite = sprite;
        _image.color = EquipColor;

        _frame.SetActive(true);
        _levelNumImage.gameObject.SetActive(true);
    }
    public void UpdateEquipmentLevel(int level)
    {
        _levelNumImage.sprite = _numsImage[level];
    }
    public void ResetInventory()
    {
        GetComponent<RectTransform>().localScale = Vector3.one / 2;
        _image.sprite = _defaultSprite;
        _image.color = DefaultColor;
        _levelNumImage.sprite = _numsImage[1];

        _frame.SetActive(false);
        _levelNumImage.gameObject.SetActive(false);
    }
}