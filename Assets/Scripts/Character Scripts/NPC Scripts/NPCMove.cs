using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove
{
    GameObject target;

    public bool moves = false;

    int d;

    void Update()
    {
        if (battleStarted)
        {
            turnNum = TurnManager.ReturnTurnNum();
            if (curHealth > maxHealth)
            {
                curHealth = maxHealth;
            }
            if (curHealth <= 0)
            {
                curHealth = 0;
            }
            if (!turn)
            {
                return;
            }
            if (!moving)
            {
                moves = true;
                FindNearestTarget();
                CalculatePath();
                FindSelectableTiles();
                actualTargetTile.target = true;
            }
            else
            {
                Move();
            }
        }
    }

    void CalculatePath()
    {
        Tile targetTile = target.GetComponent<TacticsMove>().currentTile;
        FindPath(targetTile);
    }

    void FindNearestTarget() //temporary AI for enemies to follow until full implementation
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        int distance = 16384;

        foreach (GameObject obj in targets)
        {
            if (obj.name != "Cur")
            {
                Tile t1 = obj.GetComponent<TacticsMove>().currentTile;

                FindPath(t1);

                d = GetNearest();

                if (d < distance)
                {
                    distance = d;
                    nearest = obj;
                }
            }
        }
        target = nearest;
    }
}
