using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{


    struct TileCoord
    {
        public int x;
        public int y;
        public int z;
    };

    struct Tile
    {
        TileCoord coord;
        int moveCost;
        List<Tile> neighbors;
    };

    Tile[,,] tiles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeTiles()
    {

    }

    
}
