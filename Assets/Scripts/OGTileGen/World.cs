using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Transform player;
    public Vector3 spawnPosition;

    public Material material;
    public TileType[] tileTypes;

    Chunk[,] chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];

    List<ChunkCoord> activeChunks = new List<ChunkCoord>();
    ChunkCoord prevChunkCoord;

    private void Start()
    {
        spawnPosition = new Vector3(VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth / 2f, VoxelData.ChunkHeight, VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth / 2f);
        GenerateWorld();
    }

    private void Update()
    {
        if (NewChunkCoord())
        {
            CheckViewDistance();
        }
    }

    bool NewChunkCoord()
    {
        if(GetChunkCoordFromVector3(player.position)==prevChunkCoord)
        {
            return false;
        }
        else
        {
            prevChunkCoord = GetChunkCoordFromVector3(player.position);
            return true;
        }
    }
    void GenerateWorld()
    {
        for (int x = (VoxelData.WorldSizeInChunks / 2)-VoxelData.ViewDistanceInChunks-1; x <= (VoxelData.WorldSizeInChunks / 2) +VoxelData.ViewDistanceInChunks; x++)
        {
            for (int z = (VoxelData.WorldSizeInChunks / 2) -VoxelData.ViewDistanceInChunks-1; z <= (VoxelData.WorldSizeInChunks / 2) +VoxelData.ViewDistanceInChunks; z++)
            {
                CreateNewChunk(x, z);
            }
        }
        player.position = spawnPosition;
        prevChunkCoord = GetChunkCoordFromVector3(player.position);
    }

    ChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);

        return new ChunkCoord(x, z);

    }

    void CheckViewDistance()
    {
        ChunkCoord coord = GetChunkCoordFromVector3(player.position);
        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);

        for (int x = coord.x-VoxelData.ViewDistanceInChunks; x< coord.x+ VoxelData.ViewDistanceInChunks;x++)
        {
            for (int z = coord.z - VoxelData.ViewDistanceInChunks; z < coord.z + VoxelData.ViewDistanceInChunks; z++)
            {
                if(IsChunkInWorld(new ChunkCoord(x,z)))
                {
                    if(chunks[x,z]==null)
                    {
                        CreateNewChunk(x, z);
                    }
                    else if(!chunks[x,z].isActive)
                    {
                        chunks[x, z].isActive = true;
                        activeChunks.Add(new ChunkCoord(x, z));
                    }
                }

                for(int i=0; i<previouslyActiveChunks.Count;i++)
                {
                    if(previouslyActiveChunks[i].Equals(new ChunkCoord(x,z)))
                    {
                        previouslyActiveChunks.RemoveAt(i);
                    }
                }
            }
        }

        foreach(ChunkCoord cc in previouslyActiveChunks)
        {
            chunks[cc.x, cc.z].isActive = false;
            activeChunks.Remove(cc);
        }
    }

    public byte GetVoxel(Vector3 pos)
    {
        if(!IsVoxelInWorld(pos))
        {
            return 0;
        }
        return 1;
    }

    void CreateNewChunk(int x, int z)
    {
        chunks[x, z] = new Chunk(new ChunkCoord(x, z), this);
        activeChunks.Add(new ChunkCoord(x, z));
        chunks[x, z].isActive = true;
    }

    bool IsChunkInWorld(ChunkCoord coord)
    {
        if(coord.x>0&&coord.x<VoxelData.WorldSizeInChunks-1&& coord.z > 0 && coord.z < VoxelData.WorldSizeInChunks - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsVoxelInWorld(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInChunks && pos.y >= 0 && pos.y < VoxelData.WorldSizeInChunks  && pos.z >= 0 && pos.z < VoxelData.WorldSizeInChunks )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public class TileType
{
    public string tileName;
    public bool isSolid;
}


