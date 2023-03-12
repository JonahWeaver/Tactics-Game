﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    public GameObject[] tiles;
    public List<Tile> selectableTiles = new List<Tile>();
    public List<Tile> enemyTiles = new List<Tile>();
    public List<Tile> returnTiles1 = new List<Tile>();
    public List<Tile> returnTiles2 = new List<Tile>();
    public List<Tile> returnTiles3 = new List<Tile>();
    public List<TacticsMove> starts = new List<TacticsMove>();
    public Tile currentTile;
    public Tile newCurrentTile;
    public Tile newCurrentTile2;
    Stack<Tile> path = new Stack<Tile>();
    public Vector3 temp;


    public bool turn = false;
    public bool moving = false;
    public int move = 5;
    public int attackRange = 2;
    public float jumpHeight = 2f;
    public float moveSpeed = 2;
    public int speed = 3;
    public int maxHealth;
    public int curHealth;
    public int attack;
    public int mAttack;
    public int defense;
    public int mDefense;
    public int maxMana = 0;
    public int curMana;
    public float jumpVelocity = 4.5f;
    public float minDistance;
    public Identity thisGuy;

    public int pCount = 0;

    public Vector3 velocity = new Vector3();
    public Vector3 heading = new Vector3();

    float halfHeight = 0;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;
    public int nearest1;
    public Tile actualTargetTile;
    Tile t;
    public Tile Tile;

    public int turnNum;
    public int dip;
    public bool dip2;
    public bool battleStarted = false;
    public BaseCharacter character;

    public GameObject aM;

    public void Start()

    {
        nearest1 = 0;
        foreach (GameObject menu in GameObject.FindGameObjectsWithTag("Menu"))
        {
            if (menu.name == "ActionMenu")
            {
                aM = menu;
            }
        }
    }


    protected void Init()
    {
        thisGuy = GetComponent<Identity>();
        tiles = GameObject.FindGameObjectsWithTag("Tiles");//if this becomes slow create list
                                                           //in BattleInit
        temp = transform.position;
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        TurnManager.AddUnit(this);
        GetCurrentTile();


    }

    protected void Init2()
    {
        TurnManager.AddUnit(this);
    }

    public void StartBattle()
    {
        character = thisGuy.character;

        Init();
        move = character.Move.Value;
        maxHealth = character.Health.Value;
        curHealth = maxHealth;
        battleStarted = true;
        speed = character.Speed.Value;
        maxMana = character.Mana.Value;
        curMana = maxMana;
        dip = 0;
        dip2 = true;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }
    public void GetNewCurrent(GameObject gobj)
    {
        newCurrentTile = GetTargetTile(gobj);


    }
    public void GetNewCurrent2(GameObject gobj)
    {
        newCurrentTile2 = GetTargetTile(gobj);


    }
    public Tile GetTargetTile(GameObject target)
    {
        //RaycastHit hit;
        GameObject t = BattleInit.battleInit.tiles[Mathf.RoundToInt(transform.position.x-.5f), Mathf.RoundToInt(transform.position.z - .5f)];
        Tile tile = t.GetComponent<Tile>();
        //Debug.Log(transform.position.y);
        //if (Physics.Raycast(target.transform.position, -Vector3.up, out hit,1))
        //{
        //    tile = hit.collider.GetComponent<Tile>();
        //    Debug.Log(hit.transform.name);
        //}
        return tile;
    }
    public void ComputeAdjacencyLists(float jumpheight, Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight, target);
        }
    }
    public void ComputeAdjacentEnemies()
    {

        foreach (GameObject tile in tiles)
        {

            Tile t = tile.GetComponent<Tile>();
            t.FindEnemies();
        }
    }
    public void ComputeAdjacentReturn()
    {

        foreach (GameObject tile in tiles)
        {

            Tile t = tile.GetComponent<Tile>();
            t.FindReturn();

        }
    }
    public void FindSelectableTiles()
    {

        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            if (t.inhab) continue;
            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {

                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);

                    }
                }
            }
        }
    }
    //public void FindStartTiles()
    //{
    //    List<Tile> startTiles = new List<Tile>();
    //    foreach (GameObject tile in tiles)
    //    {
    //        Tile rTile = tile.GetComponent<Tile>();
     //       if (rTile.start)
     //       {
     //           startTiles.Add(rTile);
     //       }
     //   }
//
     //   foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
  //      {
  //          if (player.name != "Cur")
  //          {
  //              TacticsMove unit = player.GetComponent<TacticsMove>();
  //              starts.Add(unit);
  //              unit.moving = true;
   //         }
    //        TurnManager.SpeedSorted(starts);
//
  //      }
   //     foreach (Tile tile in startTiles)
   //     {
   //         if (tile.startCount > starts[0].speed)
   //         {
   //             tile.start = false;
    //        }
   //     }

 //   }
    public void FindEnemyTiles()
    {

        GetNewCurrent(gameObject);
        ComputeAdjacentEnemies();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(newCurrentTile);
        newCurrentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            enemyTiles.Add(t);
            t.attackSelect = true;

            if (t.distance < attackRange)
            {
                foreach (Tile tile in t.enemyList)
                {
                    if (!tile.visited)
                    {
                        if (t.distance == 0)
                        {
                            returnTiles1.Add(tile);
                        }
                        if (t.distance == 1)
                        {
                            returnTiles2.Add(tile);
                        }
                        if (t.distance == 2)
                        {
                            returnTiles3.Add(tile);
                        }
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
        newCurrentTile.attackSelect = false;
    }

    public void UndoStartTiles()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t2 = tile.GetComponent<Tile>();
            t2.Reset();
        }
    }
    public void MoveToTile(Tile tile)
    {

        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }

    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = Mathf.Abs(transform.position.y - target.y) > 0.001f;

                if (jump)
                {
                    
                    //Debug.Log(target.y);
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = target;
                path.Pop();
            }



        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
            GetNewCurrent(gameObject);
            Tile current = newCurrentTile;
            current.inhab = this;
            current.FindInhab();
            if (gameObject.tag == "Player")
            {
                ActionMenu.StartUI();
            }
            else if (gameObject.tag == "NPC")
            {
                gameObject.GetComponent<NPCAttack>().attacking = true;
            }
            else
            {
                TurnManager.EndTurn();

            }
        }
    }
    protected void RemoveSelectableTiles()
    {

        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();

        }
        selectableTiles.Clear();
    }
    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }
    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }
    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDown(target);
        }
        else if (jumpingUp)
        {
            JumpUp(target);
        }
        else if (movingEdge)
        {
            MoveEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }
    void PrepareJump(Vector3 target)
    {
        Debug.Log(transform.position.y);
        float targetY = target.y;

        target.y = transform.position.y;

        CalculateHeading(target);
        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            minDistance = Vector3.Distance(transform.position, jumpTarget);

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
            //Debug.Log(target);




        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }
    void FallDown(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;


        if (transform.position.y <= target.y)
        {

            fallingDown = false;
            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;
            velocity = new Vector3();
        }

    }
    void JumpUp(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }
    void MoveEdge()
    {
        float distance = Vector3.Distance(transform.position, jumpTarget);

        if (distance <= minDistance)
        {
            minDistance = distance;
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }
    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }
        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return t.parent;
        }
        Tile endTile = null;
        for (int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }
        return endTile;
    }
    protected void FindPath(Tile target)
    {
        nearest1 = 0;
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);

        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {

            t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }
            foreach (Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //DO nothing; already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                    nearest1++;
                }


            }

        }

        //todo what if there is no path to target tile??

    }
    public int GetNearest()
    {
        return nearest1;
    }

    public void BeginTurn()
    {
        turn = true;
        if (transform.tag == "Player") TurnManager.PlayerTurn = true;
        else TurnManager.PlayerTurn = false;
        FindSelectableTiles();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tiles"))
        {
            Tile t = go.GetComponent<Tile>();
            t.colorChanged = true;
        }
    }
    public void EndTurn()
    {
        turn = false;
    }


}
