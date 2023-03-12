using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMove
{
    GameObject target;

    public bool moves = false;
    bool fuf;

    int d;


    // Update is called once per frame
    void Update()
    {
        if (battleStarted)
        {
            if (fuf)
            {
                maxHealth = character.Health.Value;
                curHealth = maxHealth;
                maxMana = character.Mana.Value;
                curMana = maxMana;
                jumpHeight = character.JumpHeight.Value;
                fuf = false;
            }
            turnNum = TurnManager.ReturnTurnNum();
            move = character.Move.Value;
            if (GetComponent<Inventory>().equippedWeapon != null)
            {
                attack = character.Strength.Value + GetComponent<Inventory>().equippedWeapon.atk;
                mAttack = character.Intelligence.Value + GetComponent<Inventory>().equippedWeapon.mAtk;
                attackRange = GetComponent<Inventory>().equippedWeapon.range;
            }
            mDefense = character.Will.Value;
            defense = character.Defense.Value;
            speed = character.Speed.Value;
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

        void FindNearestTarget()
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
