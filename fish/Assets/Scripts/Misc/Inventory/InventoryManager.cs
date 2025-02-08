using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] public ItemGrid selectedItemGrid;
    InventoryItem selectedItem;
    RectTransform rectTransform;

    private void Update()
    {
        if (selectedItem != null)
            rectTransform.position = Input.mousePosition;

        if (selectedItemGrid == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

            if (selectedItem == null)
            {
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
                
                if (selectedItem != null)
                    rectTransform = selectedItem.GetComponent<RectTransform>();
            }
            else
            {
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                selectedItem = null;
            }
        }
    }
}
