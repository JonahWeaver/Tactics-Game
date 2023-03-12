using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicMenu : MonoBehaviour
{
    public static TacticsMove player;
    public static GameObject MagicUI;
    public static GameObject child;
    public static bool action = false;
    public static Tile newCurrent;
    public int choice;
    public static Tile t;
    public static List<Tile> enemiesTiles = new List<Tile>();
    public static List<TacticsMove> enemies = new List<TacticsMove>();
    public Text myText;
    public static bool heal;
    public static bool magic;

    void Start()
    {
        MagicUI = gameObject;
        child = gameObject.transform.GetChild(0).gameObject;
       
        player = ActionMenu.player;
        MagicUI.SetActive(false);
        child.SetActive(false);
        choice = 0;
        heal = false;
        magic = false;

    }


    public static void StartMagicUI()
    {
        MagicUI.SetActive(true);
        child.SetActive(true);
        player = ActionMenu.player;
        action = true;
        t = player.newCurrentTile;
        enemiesTiles = ActionMenu.enemiesTiles;
        heal = false;
        magic = false;
    }

    public void GoBack1()
    {
        MagicUI.SetActive(false);
        child.SetActive(false);
        ActionMenu.StartUI();
        heal = false;
        magic = false;
        AttackMenu.attack = false;
        enemiesTiles.Clear();
        player.enemyTiles.Clear();
        if (t != null)
        {
            t.ClearEnemyTiles();
        }
        player.velocity = new Vector3();
        player.heading = new Vector3();
        choice = 0;


    }
    public void BlackMagic()
    {
        if (player.GetComponent<BaseCharacter>().bMagic)
        {

            if (player.curMana > 0)
            {
                player.enemyTiles.Clear();
                player.attackRange = 2;
                player.FindEnemyTiles();
                magic = true;
                ActionMenu.GetAttackable(player.enemyTiles);
                if (enemiesTiles.Count > 0)
                {

                    AttackMenu.StartAttackUI();
                    MagicUI.SetActive(false);
                    child.SetActive(false);

                }
                else
                {
                    ActionMenu.ComReset();
                    Debug.Log("no targets available");
                }
            }
            else
            {
                ActionMenu.ComReset();
                Debug.Log("not enough mana");
            }
        }
        else
        {
            Debug.Log("Can't use black magic");
        }

    }
    public void WhiteMagic()
    {
        if (player.GetComponent<BaseCharacter>().wMagic)
        {

            if (player.curMana > 0)
            {
                player.enemyTiles.Clear();
                heal = true;
                magic = true;
                player.attackRange = 1;
                player.FindEnemyTiles();

                ActionMenu.GetAll(player.enemyTiles);
                if (enemiesTiles.Count > 0)
                {

                    AttackMenu.StartAttackUI();
                    MagicUI.SetActive(false);
                    child.SetActive(false);

                }
                else
                {
                    ActionMenu.ComReset();
                    Debug.Log("no targets available");
                }
            }
            else
            {
                ActionMenu.ComReset();
                Debug.Log("not enough mana");
            }
        }
        else
        {
            Debug.Log("Can't use white magic");
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

