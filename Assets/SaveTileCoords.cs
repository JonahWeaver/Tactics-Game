using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SaveTileCoords
{
    [MenuItem("GameObject/Create Tile Coords")]
    static void CreateTileCoords()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create("C:/Users/Emily Rose/Tactics-Game/Assets/Resources/Data/TileCoordData.dat");
		TileCoordData data = new TileCoordData();
		data.heights = GetTileCoords(1000);
        bf.Serialize(file, data);
		file.Close();
	}

    static float[,] GetTileCoords(int worldDim)
    {
        float[,] heights;
        float acceptableCornerHeight = 0.49f;
        float acceptableMidHeight = 0.47f;
        float maxGroundAngle = 50f;
        int maxHeight = 20;
        int maxDepth = -20;
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
                if (Physics.Raycast(new Vector3(x + inw, 256, z + inw), -Vector3.up, out hit, 512))
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
                if (Physics.Raycast(new Vector3(x + 1 - inw, 256, z + inw), -Vector3.up, out hit, 512))
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
                if (Physics.Raycast(new Vector3(x + inw, 256, z + 1 - inw), -Vector3.up, out hit, 512))
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

                if (Physics.Raycast(new Vector3(x + 1 - inw, 256, z + 1 - inw), -Vector3.up, out hit, 512))
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
                if (height3 >= maxDepth && height3 <= maxHeight && !fullDiag)
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
        return heights;
    }


    static float ConvertToIndex(float coord)
    {
        return Mathf.RoundToInt(coord * 2) / 2f;
    }
}

[Serializable]
public class TileCoordData
{
	public float[,] heights;
}
