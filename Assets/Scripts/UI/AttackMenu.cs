using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenu : MonoBehaviour
{
    public static TacticsMove player;
    public static GameObject AttackUI;
    public static GameObject child;
    public bool chur;
    public static bool action = false;
    public static Tile newCurrent;
    public static int choice;
    public static Tile t;
    public static List<Tile> enemiesTiles = new List<Tile>();
    public static List<TacticsMove> enemies = new List<TacticsMove>();
    public static int ePAtk;
    public static int eEATK;
    public static int ePDef;
    public static int eEDef;
    public static int ePAcc;
    public static int eEAcc;
    public int pCurH;
    public int eCurH;
    public int pCurM;
    public int eCurM;
    public  Image playerItem;
    public  Image enemyItem;
    public Text myText;
    public Text pAtk;
    public Text pDef;
    public Text pSpd;
    public Text eAtk;
    public Text eDef;
    public Text eSpd;
    public Text pName;
    public Text eName;
    public Slider thisoldHP;
    public Slider thisoldMana;
    public Slider thisnewHP;
    public Slider thisnewMana;
    public Slider thatoldHP;
    public Slider thatoldMana;
    public Slider thatnewHP;
    public Slider thatnewMana;

    public static bool attack;
    public static int checkPlayerHit;
    public static int checkEnemyHit;
    public static List<Tile> returnTiles;

    void Start()
    {

        returnTiles = new List<Tile>();
        chur = true;
        AttackUI = gameObject;
        child = gameObject.transform.GetChild(0).gameObject;
        
        player = ActionMenu.player;
        AttackUI.SetActive(false);
        child.SetActive(false);
        choice = 0;
        attack = false;
    }
    public void getPlayerHealth(int dHP)
    {


        thisnewHP.value = player.curHealth - dHP;
        if (thisnewHP.value < 0)
        {
            thisnewHP.value = 0;
        }
        thisnewHP.maxValue = player.maxHealth;

    }
    public void getPlayerMana(int dMP)
    {

        thisnewMana.value = player.curMana - dMP;
        if (thisnewMana.value < 0)
        {
            thisnewMana.value = 0;
        }
        thisnewMana.maxValue = player.maxMana;
    }
    public void getEnemyHealth(TacticsMove en, int dHP)
    {
        thatnewHP.value = en.curHealth - dHP;
        thatnewHP.maxValue = en.maxHealth;
    }
    public void getEnemyMana(TacticsMove en, int dMP)
    {
        thatnewMana.value = en.curMana - dMP;
        thatnewMana.maxValue = en.maxMana;
    }
    void Update()
    {
        AttackChoice();
    }
    public static void StartAttackUI()
    {
        AttackUI.SetActive(true);
        child.SetActive(true);
        player = ActionMenu.player;
        action = true;
        t = player.newCurrentTile;
        enemiesTiles = ActionMenu.enemiesTiles;
        checkPlayerHit = Random.Range(1, 100);
        checkEnemyHit = Random.Range(1, 100);
        if (MagicMenu.magic)
        {
            enemiesTiles = MagicMenu.enemiesTiles;
            ePAtk = player.mAttack;
            ePAcc = 90 + 3 * player.GetComponent<Identity>().character.Skill.Value;


        }
        else
        {
            ePAtk = player.attack;
            ePAcc = player.GetComponent<Identity>().inventory.equippedWeapon.acc + 3 * player.GetComponent<Identity>().character.Skill.Value;
        }
        attack = true;
        if (enemiesTiles[choice].inhab.attackRange >= 1)
        {
            foreach (Tile tile in player.returnTiles1)
            {
                returnTiles.Add(tile);
            }
        }
        if (enemiesTiles[choice].inhab.attackRange >= 2)
        {
            foreach (Tile tile in player.returnTiles2)
            {
                returnTiles.Add(tile);
            }
        }
        if (enemiesTiles[choice].inhab.attackRange >= 3)
        {
            foreach (Tile tile in player.returnTiles3)
            {
                returnTiles.Add(tile);
            }
        }
        ActionMenu.GetReturn(enemiesTiles[choice], returnTiles);
    }

    public void GoBack()
    {
        AttackUI.SetActive(false);
        child.SetActive(false);
        ActionMenu.StartUI();
        MagicMenu.heal = false;
        MagicMenu.magic = false;
        attack = false;
        enemiesTiles.Clear();
        player.enemyTiles.Clear();
        t.ClearEnemyTiles();
        choice = 0;
        ActionMenu.returning = null;
        player.velocity = new Vector3();
        player.heading = new Vector3();
        chur = true;
        returnTiles.Clear();

    }
    public void AttackChoice()
    {
        //TacticsCam.leftRot.SetActive(false);
        //TacticsCam.rightRot.SetActive(false);
        //Debug.Log(returnTiles.Count + ", " + enemiesTiles[choice].inhab.attackRange);
        if (Input.GetKeyDown("d"))
        {
            returnTiles.Clear();

            ActionMenu.returning = null;
            choice++;
            if (choice > enemiesTiles.Count - 1)
            {
                choice = 0;
            }
            if (enemiesTiles[choice].inhab.attackRange >= 1)
            {
                foreach (Tile tile in player.returnTiles1)
                {
                    returnTiles.Add(tile);
                }
            }
            if (enemiesTiles[choice].inhab.attackRange >= 2)
            {
                foreach (Tile tile in player.returnTiles2)
                {
                    returnTiles.Add(tile);
                }
            }
            if (enemiesTiles[choice].inhab.attackRange >= 3)
            {
                foreach (Tile tile in player.returnTiles3)
                {
                    returnTiles.Add(tile);
                }
            }


            ActionMenu.GetReturn(enemiesTiles[choice], returnTiles);
            Debug.Log(returnTiles.Count + ", " + enemiesTiles[choice].inhab.attackRange);
        }
        else if (Input.GetKeyDown("a"))
        {
            returnTiles.Clear();
            ActionMenu.returning = null;
            choice--;
            if (choice < 0)
            {
                choice = enemiesTiles.Count - 1;
            }
            if (enemiesTiles[choice].inhab.attackRange >= 1)
            {
                foreach (Tile tile in player.returnTiles1)
                {
                    returnTiles.Add(tile);
                }
            }
            if (enemiesTiles[choice].inhab.attackRange >= 2)
            {
                foreach (Tile tile in player.returnTiles2)
                {
                    returnTiles.Add(tile);
                }
            }
            if (enemiesTiles[choice].inhab.attackRange >= 3)
            {
                foreach (Tile tile in player.returnTiles3)
                {
                    returnTiles.Add(tile);
                }
            }
            ActionMenu.GetReturn(enemiesTiles[choice], returnTiles);
            Debug.Log(returnTiles.Count + ", " + enemiesTiles[choice].inhab.attackRange);
        }
        foreach (Tile enemy in enemiesTiles)
        {
            enemy.attackSelect = true;
            enemy.attackTarget = false;

        }
        pName.text = player.name;
        eName.text = enemiesTiles[choice].inhab.name;
        if (MagicMenu.magic)
        {
            eEDef = enemiesTiles[choice].inhab.mDefense;
            if (MagicMenu.heal)
            {
                ePAcc = 100;
                if (player.tag == enemiesTiles[choice].inhab.tag)
                {
                    eEAcc = 0;
                }
            }
        }
        else
        {
            eEDef = enemiesTiles[choice].inhab.defense;
        }
        eEAcc = enemiesTiles[choice].inhab.GetComponent<Identity>().inventory.equippedWeapon.acc + 3 * enemiesTiles[choice].inhab.GetComponent<Identity>().character.Skill.Value;
        if (MagicMenu.heal)
        {
            eEAcc -= 50;
            ePAcc = 100;
        }
        if (eEAcc < 0)
        {
            eEAcc = 0;
        }
        pCurH = player.curHealth;
        pCurM = player.curMana;
        eCurH = enemiesTiles[choice].inhab.curHealth;
        eCurM = enemiesTiles[choice].inhab.curHealth;
        enemiesTiles[choice].attackTarget = true;
        myText.text = enemiesTiles[choice].inhab.name;
        pAtk.text = ePAtk.ToString();
        pDef.text = player.defense.ToString();
        pSpd.text = (ePAcc - 2 * enemiesTiles[choice].inhab.speed).ToString();
        eAtk.text = enemiesTiles[choice].inhab.GetComponent<TacticsMove>().attack.ToString();
        eDef.text = eEDef.ToString();
        eSpd.text = (eEAcc - 2 * player.speed).ToString();
        thisoldHP.maxValue = player.maxHealth;
        thisnewHP.maxValue = player.maxHealth;
        thatoldHP.maxValue = enemiesTiles[choice].inhab.maxHealth;
        thatnewHP.maxValue = enemiesTiles[choice].inhab.maxHealth;
        thisoldMana.maxValue = player.maxMana;
        thisnewMana.maxValue = player.maxMana;
        thatoldMana.maxValue = enemiesTiles[choice].inhab.maxMana;
        thatnewMana.maxValue = enemiesTiles[choice].inhab.maxMana;
        thisoldHP.value = player.curHealth;
        thatoldHP.value = enemiesTiles[choice].inhab.curHealth;
        thisoldMana.value = player.curMana;
        thatoldMana.value = enemiesTiles[choice].inhab.curMana;
        thatnewMana.value = enemiesTiles[choice].inhab.curMana;
        int pDMG;
        int eDMG;
        int pMDMG = 0;
        if (MagicMenu.heal)
        {
            eEAcc -= 50;
        }
        if (eEAcc < 0)
        {
            eEAcc = 0;
        }
        if (player.tag != enemiesTiles[choice].inhab.tag)
        {
            if (ActionMenu.returning != null)
            {
                if (enemiesTiles[choice].inhab.GetComponent<TacticsMove>().attack - player.defense > 1)
                {
                    eDMG = enemiesTiles[choice].inhab.GetComponent<TacticsMove>().attack - player.defense;

                }
                else
                {
                    eDMG = 1;

                }
            }
            else
            {
                eDMG = 0;

            }
        }
        else
        {
            eDMG = 0;
        }


        if (player.tag != enemiesTiles[choice].inhab.tag)
        {
            if (ePAtk - eEDef > 1)
            {
                pDMG = player.attack - enemiesTiles[choice].inhab.defense;

            }
            else
            {
                pDMG = 1;

            }
        }
        else
        {
            pDMG = 0;
        }
        if (MagicMenu.heal)
        {
            pDMG = -ePAtk;
        }

        getPlayerHealth(eDMG);
        if (MagicMenu.magic && player.curMana > 0)
        {
            pMDMG = 1;
            getPlayerMana(pMDMG);

        }
        else
        {
            pMDMG = 0;
            getPlayerMana(pMDMG);
        }
        getEnemyHealth(enemiesTiles[choice].inhab, pDMG);
        chur = false;
        playerItem.sprite = player.GetComponent<Identity>().inventory.equippedWeapon.icon;
        enemyItem.sprite = enemiesTiles[choice].inhab.GetComponent<Identity>().inventory.equippedWeapon.icon;
        if (Input.GetKeyDown("h"))
        {
            returnTiles.Clear();
            bool tr = true;
            if ((ePAcc - 2 * enemiesTiles[choice].inhab.speed) >= checkPlayerHit)
            {
                enemiesTiles[choice].inhab.curHealth -= pDMG;
            }
            else
            {
                Debug.Log(player.name + " missed");
            }
            if (enemiesTiles[choice].inhab.curHealth > 0)
            {
                if ((eEAcc - 2 * player.speed) >= checkEnemyHit)
                {
                    player.curHealth -= eDMG;
                    if (player.curHealth <= 0)
                    {
                        TurnManager.Kill(player);
                    }
                }
                else
                {
                    Debug.Log(enemiesTiles[choice].inhab.name + " missed");
                }
            }
            else
            {
                TurnManager.Kill(enemiesTiles[choice].inhab);
            }
            player.curMana -= pMDMG;
            foreach (Tile enemy in enemiesTiles)
            {
                if (tr)
                {
                    if (enemy.attackTarget && MagicMenu.heal)
                    {
                        Debug.Log(enemiesTiles[choice].inhab.name + " was healed");

                        tr = false;
                    }
                    else if (enemy.attackTarget && !MagicMenu.heal)
                    {
                        tr = false;
                    }
                }
            }
            foreach (Tile enemy in t.enemyList)
            {
                enemy.attackSelect = false;
                enemy.attackTarget = false;
            }
            MagicMenu.magic = false;
            MagicMenu.heal = false;
            t.enemyList.Clear();
            t.ClearEnemyTiles();
            enemiesTiles.Clear();
            ActionMenu.enemiesTiles.Clear();
            MagicMenu.enemiesTiles.Clear();
            AttackUI.SetActive(false);
            ActionMenu.action = false;
            MagicMenu.action = false;
            player.temp = player.transform.position;
            player.enemyTiles.Clear();
            foreach (GameObject view in ActionMenu.views)
            {
                Destroy(view);
            }
            ActionMenu.views.Clear();
            attack = false;
            returnTiles.Clear();
            choice = 0;
            //TacticsCam.rightRot.SetActive(true);
            //TacticsCam.rightRot.SetActive(true);
            ActionMenu.returning = null;
            chur = true;
            TurnManager.EndTurn();
        }

    }
    public static Tile GetNewCurrent(TacticsMove target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }
}


