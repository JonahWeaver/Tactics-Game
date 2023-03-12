using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipMenu : MonoBehaviour
{
    public static bool equip;
    public static bool swap;
    public static TacticsMove player;
    public static GameObject EquipUI;
    public static List<BaseItem> inventory;
    public static List<BaseItem> weaponsList;
    public static List<Button> buttons;
    public Tile newCurrent;
    public static Tile t;
    public Button item1;
    public Button item2;
    public Button item3;
    public Button item4;
    public Button item5;

    public static Image tempImage;
    public static bool noWeapons;
    public static bool firstWeapon;
    public static int noWeaponsCount;
    public static int nWAdd;
    public static BaseItem currentItem;
    public static int currentPlace;
    public static BaseItem equippedItem;

    public BattleController bc;
    void Start()
    {
        EquipUI = gameObject.transform.GetChild(0).gameObject;
        
        equip = false;
        inventory = new List<BaseItem>();
        weaponsList = new List<BaseItem>();
        buttons = new List<Button>();
        buttons.Add(item1);
        buttons.Add(item2);
        buttons.Add(item3);
        buttons.Add(item4);
        buttons.Add(item5);
        EquipUI.SetActive(false);
        tempImage = item5.GetComponent<Transform>().Find("ItemIcon").GetComponent<Image>();
        noWeapons = false;
        noWeaponsCount = 0;
        nWAdd = 0;
        firstWeapon = true;
        swap = false;
    }
    public static void StartEquipUI()
    {
        EquipUI.SetActive(true);

        player = ActionMenu.player;
        inventory = player.GetComponent<Inventory>().items;
        t = player.newCurrentTile;
        equip = true;
        //TacticsCam.leftRot.SetActive(false);
        //TacticsCam.rightRot.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            Image image = buttons[i].GetComponent<Transform>().Find("ItemIcon").GetComponent<Image>();
            Text text = buttons[i].GetComponent<Transform>().Find("Text").GetComponent<Text>();
            if (i < inventory.Count)
            {
                image.sprite = inventory[i].icon;
                text.text = inventory[i].name;
            }
            else
            {
                image.sprite = tempImage.sprite;
                text.text = "(Empty)";
            }
        }

    }
    public void GoBack()
    {
        EquipUI.SetActive(false);
        bc.selected = false;
        ActionMenu.StartUI();

        player.velocity = new Vector3();
        player.heading = new Vector3();
        equip = false;

    }
    public void ItemSelect()
    {
        Button currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        int index = 6;
        foreach (Button button in buttons)
        {
            if (currentButton == button)
            {
                index = buttons.IndexOf(button);

            }
        }
        if (index < inventory.Count)
        {
            currentItem = inventory[index];
            currentPlace = index;
            EquipUI.SetActive(false);
            ItemMenu.StartItemUI();
        }

    }
    void Update()
    {
        if (swap)
        {
            for (int i = 0; i < 5; i++)
            {
                Image image = buttons[i].GetComponent<Transform>().Find("ItemIcon").GetComponent<Image>();
                Text text = buttons[i].GetComponent<Transform>().Find("Text").GetComponent<Text>();
                if (i < inventory.Count)
                {
                    image.sprite = inventory[i].icon;
                    text.text = inventory[i].name;
                }
                else
                {
                    image.sprite = tempImage.sprite;
                    text.text = "(Empty)";
                }
            }
            swap = false;
        }
    }
}
