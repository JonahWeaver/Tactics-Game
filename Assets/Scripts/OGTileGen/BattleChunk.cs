using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChunk
{
    public BChunkCoord coord;

    int vertIndex = 0;
    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    public GameObject chunkObject;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public BattleWorld world;

    byte[,,] voxelMap = new byte[VoxelData.BattleChunkWidth, 2*VoxelData.BattleChunkHeight, VoxelData.BattleChunkWidth];

    public BattleChunk(BChunkCoord _coord, BattleWorld _world)
    {
        coord = _coord;
        world = _world;
        chunkObject = new GameObject();
        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();

        meshRenderer.material = world.material;
        chunkObject.transform.SetParent(world.transform);
        chunkObject.transform.position = new Vector3(coord.x * VoxelData.BattleChunkWidth, -.5f, coord.z * VoxelData.BattleChunkWidth);
        chunkObject.name = "Chunk " + coord.x + ", " + coord.z;

        PopulateVoxelMap();
        CreateMeshData();
        CreateMesh();
    }

    public Vector3 position
    {
        get { return chunkObject.transform.position; }
    }
    void PopulateVoxelMap()
    {
        
    }

    public Vector3 TCoords(int x, int y, int z)
    {
        return new Vector3(x + .5f, y + .5f, z + .5f);
    }
    void CreateMeshData()
    {
        for (int y = 0; y < VoxelData.BattleChunkHeight*2; y++)
        {
            for (int x = 0; x < VoxelData.BattleChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.BattleChunkWidth; z++)
                {
                    if (voxelMap[x, y, z] != 0)
                    {
                        AddVoxelDataToChunk(new Vector3(x, (float)(y / 2), z));
                    }
                }
            }
        }
    }

    public bool isActive
    {
        get { return chunkObject.activeSelf; }
        set { chunkObject.SetActive(value); }
    }


    bool IsVoxelInChunk(int x, int y, int z)
    {
        if (x < 0 || x > VoxelData.BattleChunkWidth - 1 || y < 0 || y > VoxelData.BattleChunkHeight - 1 || z < 0 || z > VoxelData.BattleChunkWidth - 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (!IsVoxelInChunk(x, y, z))
        {
            return world.tileTypes[world.GetVoxel(pos + position)].isSolid;
        }

        return world.tileTypes[voxelMap[x, y, z]].isSolid;
    }
    void AddVoxelDataToChunk(Vector3 pos)
    {
        for (int p = 0; p < 6; p++)
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[p]))
            {
                verts.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                verts.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                verts.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                verts.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

                uvs.Add(VoxelData.voxelUvs[0]);
                uvs.Add(VoxelData.voxelUvs[1]);
                uvs.Add(VoxelData.voxelUvs[2]);
                uvs.Add(VoxelData.voxelUvs[3]);

                tris.Add(vertIndex);
                tris.Add(vertIndex + 1);
                tris.Add(vertIndex + 2);
                tris.Add(vertIndex + 2);
                tris.Add(vertIndex + 1);
                tris.Add(vertIndex + 3);

                vertIndex += 4;
            }
        }
    }



    void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

    }
}

    public class BChunkCoord
    {
        public int x;
        public int z;

        public BChunkCoord(int _x, int _z)
        {
            x = _x;
            z = _z;
        }

        public bool Equals(BChunkCoord other)
        {
            if (other == null)
            {
                return false;
            }
            else if (other.x == x && other.z == z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

