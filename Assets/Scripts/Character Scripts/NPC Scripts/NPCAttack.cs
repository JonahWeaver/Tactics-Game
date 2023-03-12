using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    public bool attacking = false;
    public TacticsMove target;
    public List<TacticsMove> oppo = new List<TacticsMove>();
    public int max = 100;
    void Update()
    {

        if (attacking)
        {
            Debug.Log("attacking");
            Tile current = gameObject.GetComponent<TacticsMove>().newCurrentTile;
            current.FindEnemies();
            foreach (Tile enemyTile in current.enemyList)
            {
                if (enemyTile != null)
                {
                    enemyTile.FindInhab();
                    if (enemyTile.inhab != null)
                    {
                        if (current.inhab.tag != enemyTile.inhab.tag)
                        {
                            oppo.Add(enemyTile.inhab);
                        }
                    }
                }
            }
            current.enemyList.Clear();
            {
                foreach (TacticsMove opponent in oppo)
                {
                    if (opponent.curHealth < max)
                    {
                        max = opponent.curHealth;
                        target = opponent;
                    }
                }
            }
            if (target != null)
            {
                Attack(target);
                target = null;
            }
            attacking = false;
            TurnManager.EndTurn();
        }

    }

    void Attack(TacticsMove target)
    {
        Debug.Log("attacking2");
        int checkPlayerHit = Random.Range(1, 100);
        int checkEnemyHit = Random.Range(1, 100);
        int pAcc = target.GetComponent<Inventory>().equippedWeapon.acc + 3 * target.GetComponent<Identity>().character.Skill.Value;
        int eAcc = GetComponent<Inventory>().equippedWeapon.acc + 3 * GetComponent<Identity>().character.Skill.Value;
        int pDMG;
        int eDMG;
        int pMDMG = 0;

        if (GetComponent<TacticsMove>().attack - target.defense > 1)
        {
            eDMG = GetComponent<TacticsMove>().attack - target.defense;

        }
        else
        {
            eDMG = 1;

        }
        if (target.attack - GetComponent<TacticsMove>().defense > 1)
        {
            pDMG = target.attack - GetComponent<TacticsMove>().defense;

        }
        else
        {
            pDMG = 1;

        }




        if ((eAcc - 2 * target.speed) > checkEnemyHit)
        {
            target.curHealth -= eDMG;
        }
        else
        {
            Debug.Log(gameObject.name + " missed");
        }
        if (target.curHealth > 0)
        {
            if ((pAcc - 2 * GetComponent<TacticsMove>().speed) > checkPlayerHit)
            {
                GetComponent<TacticsMove>().curHealth -= pDMG;
                if (GetComponent<TacticsMove>().curHealth <= 0)
                {
                    TurnManager.Kill(GetComponent<TacticsMove>());
                }
            }
            else
            {
                Debug.Log(target.name + " missed");
            }
        }
        else
        {
            TurnManager.Kill(target);
        }
    }
}
