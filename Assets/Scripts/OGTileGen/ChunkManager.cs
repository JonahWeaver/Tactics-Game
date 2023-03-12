using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    //singleton 
    public static ChunkManager cm;

    public int chunkDim;
    public int radOfChunks;

    public int addX;
    public int addZ;

    public struct Chunk
    {
        public Tile[,] chunk;
        public int[,] chunkPos;
    }
    public Chunk chunk;
    public Chunk[,] chunks;

    int chunkBoundDown;
    int chunkBoundUp;
    public GameObject cursor;

    void Start()
    {
        chunks = new Chunk[radOfChunks, radOfChunks];
    }
    void Update()
    {
    }

    void MakeChunks(int changeChunkX, int changeChunkZ)
    {
        
    }

    void DestroyChunks()
    {

    }

    void TransArray(int changeChunkX, int changeChunkZ)
    {
        if(Mathf.Abs(changeChunkX)!=1|| changeChunkX != 0 || Mathf.Abs(changeChunkZ) != 1 || changeChunkZ != 0)
        {
            Debug.Log("Invalid Change in chunk index");
            return;
        }

        int chunkSideX;
        int chunkSideZ;

        for(int x=0; x<radOfChunks;x++)
        {
            for (int z = 0; z < radOfChunks; z++)
            {

            }
        }
    }
}
