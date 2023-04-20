using UnityEngine;
using UnityEngine.UI;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private GameObject _frame;
    [SerializeField] private Image _levelNumImage;

    [SerializeField] private Sprite[] _numsImage;

    public int ID {  get; set; }
    public void UpdateNewEquipment(Sprite sprite)
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
        
        Image image =  GetComponent<Image>();
        image.sprite = sprite;
        image.color = new Color(1, 1, 1, 1);

        _frame.SetActive(true);
        _levelNumImage.gameObject.SetActive(true);
    }
    public void UpdateEquipmentLevel(int level)
    {
        _levelNumImage.sprite = _numsImage[level];
    }
}