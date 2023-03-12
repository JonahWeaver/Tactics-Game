using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateGrid : MonoBehaviour
{

    public GameObject player;
    public List<GameObject> tiles = new List<GameObject>();
    float yRot;
    public int gridLength = 25;
    public int gridWidth = 20;
    public int gridBackLength = 5;
    public int gridHeight = 3;

    void Start()
    {
        yRot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.Space) )
        {
            DestroyGrid();
        }
    }
    void FindGridCoordinates()
    {
        int x= Mathf.RoundToInt(player.transform.position.x);
        int z= Mathf.RoundToInt(player.transform.position.z);
        yRot = player.transform.rotation.eulerAngles.y;
        int angMult = Mathf.RoundToInt(yRot / 90f);
        player.transform.position = new Vector3(x , player.transform.position.y, z );
        player.transform.eulerAngles = new Vector3(player.transform.rotation.x, angMult * 90f, player.transform.rotation.z);
    }
    void MakeGrid()
    {
  
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    for (int k = 0; k < gridHeight; k++)
                    {
                    Vector3 cubPos = player.transform.position + (transform.forward * (j - gridBackLength)) + (transform.right * (i - gridWidth / 2));
                    cubPos -= new Vector3(0, cubPos.y-k, 0);
                    cubPos = new Vector3(Mathf.RoundToInt(cubPos.x), Mathf.RoundToInt(cubPos.y), Mathf.RoundToInt(cubPos.z));
                    float dif = Mathf.RoundToInt(cubPos.y) + player.transform.position.y;
                    string t = GiveTile(Mathf.RoundToInt(cubPos.x), Mathf.RoundToInt(cubPos.z), Mathf.RoundToInt(cubPos.y), dif);

                    if (t != "Null")
                    {
                        GameObject cube = Instantiate(Resources.Load("TileTypes/" + t + "/Cube", typeof(GameObject))) as GameObject;
                        cube.name = cube.name + i + j;
                        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = cubPos;
                        tiles.Add(cube);
                    }
                    }
                }

            }

        
        

    }
    void DestroyGrid()
    {
        foreach(GameObject tile in tiles)
        { 
            Destroy(tile);
        }
        Debug.Log("Grid destroyed");
        tiles.Clear();
    }

    string GiveTile(int i, int j, int k, float dif)
    {
        string t = "";
        if (i>3 && i<=6 && j > 1 &&j <=5)
        {
            t = "Cobblestone";
        }
        else if(i > 7 && i <= 10 && j > 6 && j <= 8&&k<1)
        {
            t = "Street";
        }
        else if(dif<1)
        {
            t = "Dirt";
        }
        else
        {
            t = "Null";
        }

        return t;
    }
}
