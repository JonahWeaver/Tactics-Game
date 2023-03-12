using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    List<GameObject> tiles = new List<GameObject>();
    public int gridDim =15;
    public int worldDim = 225;
    public int addX=5;
    public int addZ = 5;
    public float acceptableCornerHeight;
    public float acceptableMidHeight;
    public float maxGroundAngle;

    public float[,] heights;

    private MeshGenerator theObject;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Destroy Tiles"))
        {
            DestroyTiles();
        }
        if(GUILayout.Button("Make Tiles"))
        {
            MakeTiles();
        }
        if (GUILayout.Button("GetTileCoords"))
        {
            GetTileCoords();
        }
    }

    void GetTileCoords()
    {
        theObject = (MeshGenerator)target;
        gridDim = theObject.gridDim;
        worldDim = theObject.worldDim;
        acceptableCornerHeight = theObject.acceptableCornerHeight;
        acceptableMidHeight = theObject.acceptableMidHeight;
        maxGroundAngle = theObject.maxGroundAngle;
        heights = new float[worldDim, worldDim];

        for (int x = 0; x < worldDim; x++)
        {
            for (int z = 0; z < worldDim; z++)
            {
                float height1 = -2;
                float height2 = -1;
                float height3 = -1;
                float height4 = -1;
                float height5 = -1;
                float groundAngle = -1;
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x , 256, z ), -Vector3.up, out hit, 512))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        height1 = Mathf.RoundToInt(hit.point.y * 2) / 2f;
                        if (Mathf.Abs(hit.point.y - height1) > acceptableCornerHeight)
                        {
                            height1 = -2;
                        }
                    }
                }
                if (Physics.Raycast(new Vector3(x  + 1, 256, z ), -Vector3.up, out hit, 512))
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
                if (Physics.Raycast(new Vector3(x  + .5f, 256, z  + .5f), -Vector3.up, out hit, 512))
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
                if (Physics.Raycast(new Vector3(x , 256, z + 1), -Vector3.up, out hit, 512))
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

                if (Physics.Raycast(new Vector3(x + 1, 256, z + 1), -Vector3.up, out hit, 512))
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
                if (height3 >= 0 && !fullDiag)
                {
                    if (groundAngle < maxGroundAngle)
                    {
                        heights[x, z] = height3;
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
    void DestroyTiles()
    {
        foreach (GameObject tile in tiles)
        {
            DestroyImmediate(tile);
        }
    }

    void MakeTiles()
    {
        theObject = (MeshGenerator)target;
        addX = theObject.addX;
        addZ = theObject.addZ;
        DestroyTiles();
        if (heights!=null)
        {
            for (int x = 0; x < gridDim; x++)
            {
                for (int z = 0; z < gridDim; z++)
                {
                    if (heights[x + addX, z + addZ] != 999)
                    {
                        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Tile tileComponent = tile.AddComponent<Tile>();
                        tileComponent.baseTile = Resources.Load("TileTypes/Cobblestone") as BaseTile;

                        tile.transform.position = new Vector3(x + .5f + addX, heights[x+ addX, z+addZ], z + .5f + addZ);
                        tile.name = "" + x + z * gridDim;
                        tile.tag = "Tile";
                        tiles.Add(tile);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Are no coords");
        }
    }
}
