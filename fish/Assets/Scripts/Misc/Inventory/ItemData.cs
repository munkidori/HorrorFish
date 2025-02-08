using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int width, height = 1; // hoeveel tiles neemt deze item in in mijn grid
    public Sprite image;
}
