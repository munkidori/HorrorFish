using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * ItemGrid.tileWidth;
        size.y = targetItem.itemData.height * ItemGrid.tileHeight;

        highlighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
        Vector2 pos = targetGrid.CalculatePosition(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);
        highlighter.localPosition = pos;
    }
}
