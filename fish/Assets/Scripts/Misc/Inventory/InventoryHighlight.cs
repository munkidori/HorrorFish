using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;
    ItemGrid parentGrid;

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.Width * parentGrid.tileSize;
        size.y = targetItem.Height * parentGrid.tileSize;

        highlighter.sizeDelta = size;
    }

    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null)
            return;

        parentGrid = targetGrid;
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        parentGrid = targetGrid;
        Vector2 pos = targetGrid.CalculateWorldPosition(targetItem, posX, posY);
        highlighter.localPosition = pos;
    }

    // overload methode voor het overlappende item highlighter
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        Vector2 pos = targetGrid.CalculateWorldPosition(targetItem, targetItem.onGridPosition.x, targetItem.onGridPosition.y);
        highlighter.localPosition = pos;
    }
}
