using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] public ItemGrid selectedItemGrid;

    private void Update()
    {
        if (selectedItemGrid == null)
            return;

        if (Input.GetMouseButtonDown(0))
            Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
        
    }
}
