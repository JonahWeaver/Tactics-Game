using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMove : TacticsMove
{
    public BattleController bc;
    // Update is called once per frame

    void Update()
    {
        if (battleStarted)
        {
            if (curHealth > maxHealth)
            {
                curHealth = maxHealth;
            }
            if (curHealth <= 0)
            {
                curHealth = 0;
            }
            if (!ActionMenu.action)
            {
                if (!turn)
                {
                }
                else if (!moving)
                {
                    FindSelectableTiles();
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

}
