using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tiles/TileTypes")]
public class BaseTile : ScriptableObject
{
    
    public enum TransType
    {
        FLOOR,
        STAIRS,
        INCLINE,
        THICKWALL
    }
    public enum MatType
    {
        STREET,
        DIRT,
        COBBLESTONE
    }
    public TransType transType;
    public MatType matType;
    public Color theColor;
    public int moveCost;
    public int height;
    public bool underTiles;
    
}

