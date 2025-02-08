using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] public ItemGrid selectedItemGrid;
    InventoryItem selectedItem;

    private void Update()
    {
        if (selectedItemGrid == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

            if (selectedItem == null)
            {
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
            }
            else
            {
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
            }
        }
    }
}
