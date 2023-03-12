using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMove : TacticsMove
{
    bool fuf=true;
    public BattleController bc;
    // Update is called once per frame
    void Update()
    {
        if (battleStarted)
        {
            if (fuf)
            {
                if (GetComponent<Inventory>().equippedWeapon != null)
                {
                    attack = character.Strength.Value + GetComponent<Inventory>().equippedWeapon.atk;
                    mAttack = character.Intelligence.Value + GetComponent<Inventory>().equippedWeapon.mAtk;
                    attackRange = GetComponent<Inventory>().equippedWeapon.range;
                    jumpHeight = character.JumpHeight.Value;
                }
                defense = character.Defense.Value;
                mDefense = character.Will.Value;
                turnNum = TurnManager.ReturnTurnNum();
                fuf = false;
            }
            if (curHealth > maxHealth)
            {
                curHealth = maxHealth;
            }
            if (curHealth <= 0)
            {
                curHealth = 0;
            }
            if (turn && turnNum == 1 && dip > 2 && dip2)
            {
                moving = false;
                dip2 = false;

            }
            else if (turn && dip2)
            {
                dip++;
                return;

            }
            if (!ActionMenu.action)
            {
                if (!turn)
                {
                }
                else if (!moving)
                {
                    FindSelectableTiles();
                    //CheckMouse();
                    if(bc.selected)
                    {
                        MoveToTile(bc.moveTile);
                    }
                }
                else
                {
                    Move();
                }
            }
        }
    }
    //void CheckMouse()
    //{
    //    if (turn)
    //    {
    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //            RaycastHit hit;
    //            if (Physics.Raycast(ray, out hit))
    //            {
    //                if (hit.collider.tag == "Tile")
    //                {
    //                    Tile t = hit.collider.GetComponent<Tile>();
    //                    if (t.selectable)
    //                    {
    //                        MoveToTile(t);

    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

}
