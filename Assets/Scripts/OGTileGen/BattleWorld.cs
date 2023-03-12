using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWorld : MonoBehaviour
{
    public Transform player;
    public Transform cursor;

    public Material material;
    public TileType[] tileTypes;

    BattleChunk[,] bChunks = new BattleChunk[VoxelData.BattleWorldSizeInChunks, VoxelData.BattleWorldSizeInChunks];

    List<BChunkCoord> activeBChunks = new List<BChunkCoord>();
    BChunkCoord prevBChunkCoord;

    private void Awake()
    {
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
        if (GetChunkCoordFromVector3(cursor.position).x == prevBChunkCoord.x && GetChunkCoordFromVector3(cursor.position).z == prevBChunkCoord.z)
        {
            return false;
        }
        else
        {
            prevBChunkCoord = GetChunkCoordFromVector3(cursor.position);
            return true;
        }
    }
    void GenerateWorld()
    {
        int newX = Mathf.RoundToInt(player.position.x - .5f);
        int newZ = Mathf.RoundToInt(player.position.z - .5f);
        player.position = new Vector3(newX + .5f, BattleInit.battleInit.heights[newX, newZ]+.42f, newZ + .5f);

        BChunkCoord curCoord = GetChunkCoordFromVector3(player.position);

        for (int x = curCoord.x - VoxelData.BattleViewDistanceInChunks; x <= curCoord.x + VoxelData.BattleViewDistanceInChunks; x++)
        {
            for (int z = curCoord.z- VoxelData.BattleViewDistanceInChunks; z <= curCoord.z + VoxelData.BattleViewDistanceInChunks; z++)
            {
                CreateNewBattleChunk(x, z);
            }
        }
        prevBChunkCoord = GetChunkCoordFromVector3(player.position);
    }

    BChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.BattleChunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.BattleChunkWidth);

        return new BChunkCoord(x, z);

    }

    void CheckViewDistance()
    {
        BChunkCoord coord = GetChunkCoordFromVector3(cursor.position);
        List<BChunkCoord> previouslyActiveChunks = new List<BChunkCoord>(activeBChunks);

        for (int x = (coord.x - VoxelData.BattleViewDistanceInChunks); x <= coord.x + VoxelData.BattleViewDistanceInChunks; x++)
        {
            for (int z = (coord.z - VoxelData.BattleViewDistanceInChunks); z <= coord.z + VoxelData.BattleViewDistanceInChunks; z++)
            {
                if (IsChunkInWorld(new BChunkCoord(x, z)))
                {
                    if (bChunks[x, z] == null)
                    {
                        CreateNewBattleChunk(x, z);
                    }
                    else if (!bChunks[x, z].isActive)
                    {
                        bChunks[x, z].isActive = true;
                        activeBChunks.Add(new BChunkCoord(x, z));
                    }
                }

                for (int i = 0; i < previouslyActiveChunks.Count; i++)
                {
                    if (previouslyActiveChunks[i].Equals(new BChunkCoord(x, z)))
                    {
                        previouslyActiveChunks.RemoveAt(i);
                    }
                }
            }
        }

        foreach (BChunkCoord cc in previouslyActiveChunks)
        {
            bChunks[cc.x, cc.z].isActive = false;
            activeBChunks.Remove(cc);
        }
    }

    public byte GetVoxel(Vector3 pos)
    {
        float height = BattleInit.battleInit.heights[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)];
        if (!IsVoxelInWorld(pos))
        {
            return 0;
        }
        return 1;
    }

    void CreateNewBattleChunk(int x, int z)
    {
        bChunks[x, z] = new BattleChunk(new BChunkCoord(x, z), this);
        activeBChunks.Add(new BChunkCoord(x, z));
        bChunks[x, z].isActive = true;
    }

    bool IsChunkInWorld(BChunkCoord coord)
    {
        if (coord.x > 0 && coord.x < VoxelData.BattleWorldSizeInChunks-1 && coord.z > 0 && coord.z < VoxelData.BattleWorldSizeInChunks-1)
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
        if (pos.x >= 0 && pos.x < VoxelData.BattleWorldSizeInVoxels && pos.y < 0 && pos.y > VoxelData.BattleChunkHeight && pos.z >= 0 && pos.z < VoxelData.BattleWorldSizeInVoxels)
        {
            //^
            //|
            //pos.y<0 AND pos.y> VoxelData.BattleChunkHeight?? Both can't happen but it makes it work?
            //Maybe y ends up negative?? But how does ChunkHeight end up negative?
            return true;
        }
        else
        {
            return false;
        }
    }
}
