using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BaseTile baseTile;
    public Color OGColor;
    public bool colorChanged;

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool attackTarget = false;
    public bool attackPresent = false;
    public bool attackSelect = false;
    public bool selectable = false;
    public bool returning = false;
    public bool test = false;
    //public bool start = false;
    public bool cursor = false;
    public bool high = true;
    //temporary variable; will delete later
    public bool corner = false;
    public List<Tile> adjacencyList = new List<Tile>();
    public List<Tile> enemyList = new List<Tile>();
    public List<Tile> enemyList2 = new List<Tile>();
    public List<Tile> returnList = new List<Tile>();

    public bool visited = false;
    public bool visited2 = false;
    public Tile parent = null;
    public Tile parent2 = null;
    public int distance = 0;
    public int distance2 = 0;
    public int startCount = 0;

    public Ray ray;

    public float f = 0;
    public float g = 0;
    public float h = 0;
    public float r;

    

    Vector3 halfExtents;
    public List<TacticsMove> enemy;
    public TacticsMove inhab;

    struct tileCoord
    {
        public int x;
        public int y;
        public int z;
    };
    void Awake()
    {
        colorChanged = true;
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.enabled = false;
        //MeshFilter filter = GetComponent<MeshFilter>();
        //Destroy(filter);
    }


    void Update()
    {
        if (colorChanged)
        {
            if (baseTile != null)
            {
                OGColor = baseTile.theColor;
                GetComponent<Renderer>().material.SetColor("_Color", OGColor);
            }
            if (current)
            {
                if (cursor) GetComponent<Renderer>().material.color = Color.green;
                else GetComponent<Renderer>().material.color = Color.magenta;
            }
            
            else if (selectable)
            {
                if (cursor) GetComponent<Renderer>().material.color = Color.yellow;
                else GetComponent<Renderer>().material.color = Color.blue;
            }
            else if (attackTarget)
            {

                GetComponent<Renderer>().material.color = Color.red;
            }
            else if (attackPresent)
            {

                GetComponent<Renderer>().material.color = Color.green;

            }


            else if (attackSelect)
            {

                GetComponent<Renderer>().material.color = Color.cyan;

            }
            else if (target)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            
            
            else if (returning)
            {

            }
            else if(corner)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<Renderer>().material.color = OGColor;
            }
            colorChanged = false;
        }

    }
    public void Reset()
    {
        adjacencyList.Clear();
        enemyList.Clear();
        returnList.Clear();
        current = false;
        target = false;
        selectable = false;
        high = true;
        attackTarget = false;
        attackSelect = false;
        attackPresent = false;
        returning = false;
        //cursor = false;
        corner = false;
        //start = false;
        test = false;
        visited2 = false;
        visited = false;
        parent = null;
        distance = 0;
        distance2 = 0;
        f = 0;
        g = 0;
        h = 0;
    }
    public void SReset()
    {
        adjacencyList.Clear();
        enemyList.Clear();
        returnList.Clear();
        current = false;
        target = false;
        selectable = false;
        high = true;
        returning = false;
        //start = false;
        test = false;
        visited2 = false;
        visited = false;
        parent = null;
        distance = 0;
        distance2 = 0;
        f = 0;
        g = 0;
        h = 0;
    }
    public void FindNeighbors(float jumpHeight, Tile target)
    {
        //Debug.Log("FN");
        Reset();
        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
    }
    public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {

        halfExtents = new Vector3(.25f, jumpHeight, .25f);

        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        RaycastHit hit3;

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null)
            {
                if (tile.transform.position.y > transform.position.y)
                {
                    ray = new Ray(transform.position + new Vector3(0f, 1f, 0f), Vector3.up);
                    RaycastHit hit2;
                    if (Physics.Raycast(ray, out hit2))
                    {
                        if (hit2.distance <= .5f + tile.transform.position.y)
                        {
                            high = false;
                        }
                    }
                }
                else if (tile.transform.position.y < transform.position.y)
                {
                    ray = new Ray(transform.position + new Vector3(0f, 1f, 0f), direction);
                    RaycastHit hit2;
                    if (Physics.Raycast(ray, out hit2))
                    {
                        if (hit2.distance <= .5f)
                        {
                            high = false;
                        }
                    }
                    if (high)
                    {
                        ray = new Ray(tile.transform.position, Vector3.up);
                        if (Physics.Raycast(ray, out hit2))
                        {
                            if (hit2.collider.transform.position.y == transform.position.y)
                            {
                                high = false;
                            }
                        }
                    }
                }

            }
            if (tile != null && tile.walkable && high)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                {
                    adjacencyList.Add(tile);
                }
            }
            high = true;
        }
    }
    public void FindEnemies()
    {
        Reset();
        CheckTileForEnemy(Vector3.forward);
        CheckTileForEnemy(-Vector3.forward);
        CheckTileForEnemy(Vector3.right);
        CheckTileForEnemy(-Vector3.right);

    }
    public void CheckTileForEnemy(Vector3 direction)
    {

        halfExtents = new Vector3(.25f, 1f, .25f);

        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                enemyList.Add(tile);

            }

        }
    }
    public void CheckForEnemy()
    {

        ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit2;
        if (Physics.Raycast(ray, out hit2))
        {

            enemy.Add(hit2.collider.GetComponent<TacticsMove>());


            attackTarget = false;
            attackSelect = true;
            enemyList.Add(this);




        }
        enemyList2 = enemyList;
    }
    public void ClearEnemyTiles()
    {
        Reset();
        CheckTileToClear(Vector3.forward);
        CheckTileToClear(-Vector3.forward);
        CheckTileToClear(Vector3.right);
        CheckTileToClear(-Vector3.right);
    }
    public void CheckTileToClear(Vector3 direction)
    {
        halfExtents = new Vector3(.25f, 1f, .25f);

        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null)
            {
                ray = new Ray(tile.transform.position, Vector3.up);
                RaycastHit hit4;
                if (Physics.Raycast(ray, out hit4))
                {
                    if (hit4.collider.tag == "NPC")
                    {

                        tile.attackTarget = false;
                        tile.attackSelect = false;
                    }
                }
            }
        }
    }
    public void FindReturn()
    {
        SReset();
        CheckReturn(Vector3.forward);
        CheckReturn(-Vector3.forward);
        CheckReturn(Vector3.right);
        CheckReturn(-Vector3.right);
    }
    public void CheckReturn(Vector3 direction)
    {
        halfExtents = new Vector3(.25f, .25f, .25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                returnList.Add(tile);
            }
        }
    }
    public void FindInhab()
    {
        halfExtents = new Vector3(0, 1f, 0);

        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtents);

        foreach (Collider item in colliders)
        {
            if (item.tag == "NPC" || item.tag == "Player")
            {
                inhab = item.GetComponent<TacticsMove>();
            }
        }
    }
}
