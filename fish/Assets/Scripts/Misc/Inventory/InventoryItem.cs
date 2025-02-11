using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;
    public int Height 
    { 
        get
        {
            if (rotated == false)
                return itemData.height;
            else
                return itemData.width;
        } 
    }
    public int Width
    {
        get
        {
            if (rotated == false)
                return itemData.width;
            else
                return itemData.height;
        }
    }
    public int onGridPositionX, onGridPositionY;
    public bool rotated = false;

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;
        GetComponent<Image>().sprite = itemData.image;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileWidth;
        size.y = itemData.height * ItemGrid.tileHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }

    internal void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }
}
