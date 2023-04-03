using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

public class BattleInit : MonoBehaviour
{
    public static BattleInit battleInit;
    public float acceptableCornerHeight = .49f;
    public float acceptableMidHeight = .47f;
    public float maxGroundAngle = 50f;
    public int maxHeight = 20;
    public int maxDepth = -20;

    public GameObject[,] tiles;
    public float[,] heights;

    void Awake()
    {
        //this is causing the bootup issues. 
        battleInit = this;
        LoadTileCoordData();
        tiles = new GameObject[1000, 1000];//placeholder for worldDim atm
    }

    void LoadTileCoordData()
    {
        if (File.Exists("C:/Users/Emily Rose/Tactics-Game/Assets/Resources/Data/TileCoordData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open("C:/Users/Emily Rose/Tactics-Game/Assets/Resources/Data/TileCoordData.dat", FileMode.Open);
            TileCoordData data = (TileCoordData)bf.Deserialize(file);
            file.Close();
            heights = data.heights;
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
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
}
