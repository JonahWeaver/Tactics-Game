using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInit : MonoBehaviour
{
    public static BattleInit battleInit;
    public float acceptableCornerHeight;
    public float acceptableMidHeight;
    public float maxGroundAngle;
    public int maxHeight;
    public int maxDepth;

    int vertIndex = 0;
    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public GameObject[,] tiles;
    public float[,] heights;
    BattleWorld world;

    byte[,,] voxelMap = new byte[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];
    void Awake()
    {
        battleInit = this;
        GetTileCoords(1000);
        tiles = new GameObject[1000, 1000];//placeholder for worldDim atm
    }

    void Start()
    {
        
    }

    public void GetTileCoords(int worldDim)
    {
        heights = new float[worldDim, worldDim];
        for (int x = 0; x < worldDim; x++)
        {
            for (int z = 0; z < worldDim; z++)
            {
                //THE INDEXES OF EACH TILE IS ADD BY 0.5 TO GET THEIR WORLD POSITION
                float height1 = -2;
                float height2 = -1;
                float height3 = -1;
                float height4 = -1;
                float height5 = -1;
                float inw = .1f;
                float groundAngle = -1;
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x+inw, 256, z+inw), -Vector3.up, out hit, 512))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        height1 = Mathf.RoundToInt(hit.point.y * 2) / 2f;
                        if (Mathf.Abs(hit.point.y - height1) > acceptableCornerHeight)
                        {
                            height1 = -1;
                        }
                    }
                }
                if (Physics.Raycast(new Vector3(x + 1-inw, 256, z+inw), -Vector3.up, out hit, 512))
                {

                    if (hit.transform.tag == "Ground")
                    {
                        height2 = Mathf.RoundToInt(hit.point.y * 2) / 2f;
                        if (Mathf.Abs(hit.point.y - height2) > acceptableCornerHeight)
                        {
                            height2 = -1;
                        }
                    }
                }
                if (Physics.Raycast(new Vector3(x + .5f, 256, z + .5f), -Vector3.up, out hit, 512))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        height3 = Mathf.RoundToInt(hit.point.y * 2) / 2f;
                        if (Mathf.Abs(hit.point.y - height3) > acceptableMidHeight)
                        {
                            height3 = -1;
                        }
                        groundAngle = Mathf.RoundToInt(Vector3.Angle(hit.normal, Vector3.up));
                    }
                }
                if (Physics.Raycast(new Vector3(x+inw, 256, z + 1-inw), -Vector3.up, out hit, 512))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        height4 = Mathf.RoundToInt(hit.point.y * 2) / 2f;
                        if (Mathf.Abs(hit.point.y - height4) > acceptableCornerHeight)
                        {
                            height4 = -1;
                        }
                    }
                }

                if (Physics.Raycast(new Vector3(x + 1-inw, 256, z + 1-inw), -Vector3.up, out hit, 512))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        height5 = Mathf.RoundToInt(hit.point.y * 2) / 2f;
                        if (Mathf.Abs(hit.point.y - height5) > acceptableCornerHeight)
                        {
                            height5 = -1;
                        }
                    }
                }
                bool greatDiagonals1 = Mathf.Abs(height1 - height5) > 1;
                bool greatDiagonals2 = Mathf.Abs(height2 - height4) > 1;
                bool fullDiag = greatDiagonals1 || greatDiagonals2;
                if (height3 >= maxDepth && height3<=maxHeight && !fullDiag)
                {
                    if (groundAngle < maxGroundAngle)
                    {
                        heights[x, z] = ConvertToIndex(height3);
                    }
                    else
                    {
                        heights[x, z] = 999;
                    }
                }
                else
                {
                    heights[x, z] = 999;
                }
            }
        }
    }


    public float ConvertToIndex(float coord)
    {
        return Mathf.RoundToInt(coord*2)/2f;
    }

    public void MakeTiles(int addX, int addZ, int worldDim, GameObject player)
    {
        foreach(GameObject unit in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(unit.transform.position.x>=addX&& unit.transform.position.x <worldDim+addX&& unit.transform.position.z >= addZ && unit.transform.position.z < worldDim + addZ)
            {
                int rot = 90 * (Mathf.RoundToInt(unit.transform.rotation.eulerAngles.y / 90));
                unit.transform.eulerAngles = new Vector3(0, rot, 0);
                int x = Mathf.RoundToInt(unit.transform.position.x - .5f);
                int z = Mathf.RoundToInt(unit.transform.position.z - .5f);
                unit.transform.position = new Vector3(x + .5f, BattleInit.battleInit.heights[x, z] + .92f, z + .5f);
            }
        }
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("NPC"))
        {
            if (unit.transform.position.x >= addX && unit.transform.position.x < worldDim + addX && unit.transform.position.z >= addZ && unit.transform.position.z < worldDim + addZ)
            {
                int rot = 90 * (Mathf.RoundToInt(unit.transform.rotation.eulerAngles.y / 90));
                unit.transform.eulerAngles = new Vector3(0, rot, 0);
                int x = Mathf.RoundToInt(unit.transform.position.x - .5f);
                int z = Mathf.RoundToInt(unit.transform.position.z - .5f);
                unit.transform.position = new Vector3(x + .5f, BattleInit.battleInit.heights[x, z] + .92f, z + .5f);
            }
        }
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("FNPC"))
        {
            if (unit.transform.position.x >= addX && unit.transform.position.x < worldDim + addX && unit.transform.position.z >= addZ && unit.transform.position.z < worldDim + addZ)
            {
                int rot = 90 * (Mathf.RoundToInt(unit.transform.rotation.eulerAngles.y / 90));
                unit.transform.eulerAngles = new Vector3(0, rot, 0);
                int x = Mathf.RoundToInt(unit.transform.position.x - .5f);
                int z = Mathf.RoundToInt(unit.transform.position.z - .5f);
                unit.transform.position = new Vector3(x + .5f, BattleInit.battleInit.heights[x, z] + .92f, z + .5f);
            }
        }

        if (heights != null)
        {
           for (int x = 0; x < worldDim; x++)
            {
                for (int z = 0; z < worldDim; z++)
                {
                    if (heights[x + addX, z + addZ] != 999)
                    {
                        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Tile tileComponent = tile.AddComponent<Tile>();
                        tileComponent.baseTile = Resources.Load("TileTypes/Cobblestone") as BaseTile;
                        tile.GetComponent<MeshRenderer>().material = Resources.Load("Materials/VoxelMat") as Material;
                        
                        tile.transform.position = new Vector3(x + .5f + addX, heights[x + addX, z + addZ], z + .5f + addZ);
                        //tile.GetComponent<MeshRenderer>().enabled = false;
                        tile.name = "" + x + " " + z;
                        tile.tag = "Tiles";
                        //tile.layer= LayerMask.NameToLayer("Tile");
                        tile.AddComponent<Rigidbody>();
                        Destroy(tile.GetComponent<Rigidbody>());
                        tiles[x + addX, z + addZ] = tile;
                        //tiles[1, height
                    }
                }
           }
       }
       else
        { 
            Debug.Log("Are no coords");
        }
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (unit.transform.position.x >= addX && unit.transform.position.x < worldDim + addX && unit.transform.position.z >= addZ && unit.transform.position.z < worldDim + addZ)
            {
                unit.GetComponent<TacticsMove>().StartBattle();
                unit.GetComponent<Rigidbody>().useGravity = false;
                unit.GetComponent<Rigidbody>().detectCollisions = false;
            }
        }
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("NPC"))
        {
            if (unit.transform.position.x >= addX && unit.transform.position.x < worldDim + addX && unit.transform.position.z >= addZ && unit.transform.position.z < worldDim + addZ)
            {
                unit.GetComponent<TacticsMove>().StartBattle();
                unit.GetComponent<Rigidbody>().useGravity = false;
                unit.GetComponent<Rigidbody>().detectCollisions = false;
            }
        }
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("FNPC"))
        {
            if (unit.transform.position.x >= addX && unit.transform.position.x < worldDim + addX && unit.transform.position.z >= addZ && unit.transform.position.z < worldDim + addZ)
            {
                unit.GetComponent<TacticsMove>().StartBattle();
                unit.GetComponent<Rigidbody>().useGravity = false;
                unit.GetComponent<Rigidbody>().detectCollisions = false;
            }
        }

    }

    

    public void ReplaceTiles(int addX, int addZ, int modX, int modZ,int gridDim)
    {
        
    }

    void PopulateVoxelMap()
    {
        for (int y = 0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    voxelMap[x, y, z] = 0;
                }
            }
        }
    }

    void CreateMeshData()
    {
        for (int y = 0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    AddVoxelDataToChunk(new Vector3(x, y, z));
                }
            }
        }
    }

    bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (x < 0 || x > VoxelData.ChunkWidth - 1 || y < 0 || y > VoxelData.ChunkHeight - 1 || z < 0 || z > VoxelData.ChunkWidth - 1)
        {
            return false;
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
