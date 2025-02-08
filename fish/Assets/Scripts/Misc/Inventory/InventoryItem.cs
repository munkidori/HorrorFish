using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;
    public int onGridPositionX, onGridPositionY;

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;
        GetComponent<Image>().sprite = itemData.image;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileWidth;
        size.y = itemData.height * ItemGrid.tileHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }
}
