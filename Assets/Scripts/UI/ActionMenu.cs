using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public static TacticsMove player;
    public static GameObject ActionUI;
    public static GameObject child;
    public static List<GameObject> views = new List<GameObject>();
    public static GameObject eqView;
    public static bool action = false;
    public static Tile newCurrent;
    public static TacticsMove enemy;
    public static Tile t;
    public static List<Tile> enemiesTiles = new List<Tile>();
    public static List<TacticsMove> enemies = new List<TacticsMove>();
    public static TacticsMove returning;

    public static int curButton;

    void Start()
    {
        eqView = new GameObject();
        eqView.name = "EQView";
        ActionUI = gameObject;
        child = gameObject.transform.GetChild(0).gameObject;

        ActionUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (curButton == 0) curButton = 5;
            else curButton--;
            GameObject gButton = child.transform.GetChild(curButton).gameObject;
            EventSystem.current.SetSelectedGameObject(gButton);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (curButton == 5) curButton = 0;
            else curButton++;
            GameObject gButton = child.transform.GetChild(curButton).gameObject;
            EventSystem.current.SetSelectedGameObject(gButton);
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            Button gButton = child.transform.GetChild(curButton).gameObject.GetComponent<Button>();
            gButton.onClick.Invoke();
        }
    }
    public static void StartUI()
    {
        foreach (GameObject view in views)
        {
            Destroy(view);
        }
        views.Clear();
        player = TurnManager.GetActivePlayer();
        player.attackRange = player.GetComponent<Inventory>().equippedWeapon.range;

        ActionUI.SetActive(true);
        child.SetActive(true);
        curButton = 0;

        GameObject gButton= child.transform.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(gButton);

        action = true;
        ComReset();
    }

    public void MakeButtons()
    {

    }
    public void Attack()
    {
        List<TacticsMove> enemies = new List<TacticsMove>();
        player.returnTiles1.Clear();
        player.returnTiles2.Clear();
        player.returnTiles3.Clear();
        player.FindEnemyTiles();

        GetAttackable(player.enemyTiles);
        if (enemiesTiles.Count > 0)
        {
            ActionUI.SetActive(false);
            child.SetActive(false);
            AttackMenu.StartAttackUI();

        }
        else
        {
            Debug.Log("no targets available");
            ComReset();
        }
    }
    public void Magic()
    {
        MagicMenu.StartMagicUI();
        ActionUI.SetActive(false);
        child.SetActive(false);
    }
    public void Equipment()
    {
        EquipMenu.StartEquipUI();
        GetEquipViews();
        ActionUI.SetActive(false);
        child.SetActive(false);
    }
    public void Stats()
    {

        GetEquipViews();
        ActionUI.SetActive(false);
        child.SetActive(false);
    }
    public void Wait()

    {
        ActionUI.SetActive(false);
        child.SetActive(false);
        action = false;
        player.temp = player.transform.position;
        TurnManager.EndTurn();

    }
    public void GoBack()
    {
        ActionUI.SetActive(false);
        child.SetActive(false);
        player.transform.position = player.temp;
        player.moving = false;
        player.GetComponent<BattleController>().selected = false;
        action = false;
        player.velocity = new Vector3();
        player.heading = new Vector3();

        player.newCurrentTile.inhab = default;

        //GetComponent<TacticsMove>().GetNewCurrent(gameObject);
        //Tile current = GetComponent<TacticsMove>().newCurrentTile;
        //current.inhab = GetComponent<TacticsMove>();
        //current.FindInhab();
        //if "go back" is pressed where a tile is, player still travels to tile
    }
    public static void GetAttackable(List<Tile> enemie)
    {

        foreach (Tile tile in enemie)
        {
            if (tile.inhab&&tile.inhab.tag == "NPC")
            {
                enemiesTiles.Add(tile);
                tile.attackPresent = true;
                GameObject view = new GameObject();
                view.tag = "View";
                view.transform.position = tile.inhab.transform.position + new Vector3(0f, 2f, 0f);

                view.transform.RotateAround(player.transform.position, Vector3.up, 135f);
                view.transform.LookAt(tile.inhab.transform);
                views.Add(view);
            }
            //    Ray ray = new Ray(ty.transform.position, Vector3.up);
            //    RaycastHit hit4;
            //    if (Physics.Raycast(ray, out hit4))
            //    {
            //        if (hit4.collider.tag == "NPC")
            //        {
            //            Debug.Log(hit4.collider.name);
            //            ty.inhab = hit4.collider.GetComponent<TacticsMove>();
            //            enemiesTiles.Add(ty);
            //            ty.attackPresent = true;
            //            GameObject view = new GameObject();
            //            view.tag = "View";
            //            view.transform.position = ty.inhab.transform.position + new Vector3(0f, 2f, 0f);

            //            view.transform.RotateAround(player.transform.position, Vector3.up, 135f);
            //            view.transform.LookAt(ty.inhab.transform);
            //            views.Add(view);

            //        }
            //    }



            //}
        }
    }
    public static void GetReturn(Tile playerTile, List<Tile> returns)
    {
        foreach (Tile tile in returns)
        {
            Ray ray = new Ray(tile.transform.position, Vector3.up);
            RaycastHit hit4;
            if (playerTile == tile)
            {

                if (Physics.Raycast(ray, out hit4))
                {
                    if (hit4.collider.name == playerTile.inhab.name)
                    {
                        returning = player;
                    }
                    else
                    {
                        returning = null;
                    }

                }
            }




        }
    }
    public static void GetAll(List<Tile> enemie)
    {

        foreach (Tile tile in enemie)
        {
            Tile ty = tile.GetComponent<Tile>();
            Ray ray = new Ray(ty.transform.position, Vector3.up);
            RaycastHit hit4;
            if (Physics.Raycast(ray, out hit4))
            {
                if (hit4.collider.tag == "NPC" && ty != player.newCurrentTile || hit4.collider.tag == "Player" && ty != player.newCurrentTile)
                {
                    ty.inhab = hit4.collider.GetComponent<TacticsMove>();
                    MagicMenu.enemiesTiles.Add(ty);
                    ty.attackPresent = true;
                    GameObject view = new GameObject();
                    view.tag = "View";
                    view.transform.position = ty.inhab.transform.position + new Vector3(0f, 2f, 0f);

                    view.transform.RotateAround(player.transform.position, Vector3.up, 135f);
                    view.transform.LookAt(ty.inhab.transform);
                    views.Add(view);
                }

            }



        }
    }
    public static void GetEquipViews()
    {

        eqView.transform.position = player.transform.position + (3.3f * Vector3.forward);
        eqView.transform.rotation = player.transform.rotation;

        eqView.transform.RotateAround(player.transform.position, Vector3.up, 45f);
        eqView.transform.LookAt(player.transform);

    }
    public static void ComReset()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tiles");
        foreach (GameObject tile in tiles)
        {
            Tile ty = tile.GetComponent<Tile>();
            ty.Reset();

        }
    }

}

