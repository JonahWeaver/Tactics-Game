using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{
    public static GameObject ItemUI;
    public static GameObject child;
    public static BaseItem currentItem;
    public static TacticsMove player;
    public static int currentPlace;
    public bool item;
    public Image playerIcon;
    public Image ItemIcon;
    public Text ItemName;
    public Text ItemDesc;
    public Text HPMod;
    public Text Uses;
    public Text playerName;
    public Text attack;
    public Text defense;
    public Text useEquip;
    public static Slider oldHP;
    public static Slider newHP;
    public static Slider oldMana;
    public static Slider newMana;
    void Start()
    {

        ItemUI = gameObject;
        child = gameObject.transform.GetChild(0).gameObject;
        foreach (GameObject sliderObj in GameObject.FindGameObjectsWithTag("Slider"))
        {
            Slider slider = sliderObj.GetComponent<Slider>();
            if (sliderObj.name == "PNewHPItem")
            {
                newHP = slider;

            }
            else if (sliderObj.name == "POldHPItem")
            {
                oldHP = slider;
            }
            else if (sliderObj.name == "PNewManaItem")
            {
                newMana = slider;
            }
            else if (sliderObj.name == "POldManaItem")
            {
                oldMana = slider;
            }
        }
        ItemUI.SetActive(false);
        child.SetActive(false);
        item = false;


    }

    public static void StartItemUI()
    {
        ItemUI.SetActive(true);
        child.SetActive(true); ;
        player = EquipMenu.player;
        currentItem = EquipMenu.currentItem;
        currentPlace = EquipMenu.currentPlace;
        oldHP.maxValue = newHP.maxValue = player.maxHealth;
        oldMana.maxValue = newMana.maxValue = player.maxMana;
        oldHP.value = player.curHealth;
        oldMana.value = player.curMana;
        if (currentItem.type == BaseItem.ItemType.POTION)
        {
            if (player.curHealth < player.maxHealth)
            {
                if (currentItem.hRestore > 0)
                {
                    newHP.value = oldHP.value + currentItem.hRestore;
                    if (newHP.value > newHP.maxValue)
                    {
                        newHP.value = newHP.maxValue;
                    }
                }
                else if (currentItem.mRestore > 0)
                {
                    newMana.value = oldMana.value + currentItem.mRestore;
                    if (newMana.value > newMana.maxValue)
                    {
                        newMana.value = newMana.maxValue;
                    }
                }
            }
        }
    }
    public void Use()
    {
        if (currentItem.type == BaseItem.ItemType.WEAPON)
        {
            BaseWeapon currentWeapon = currentItem as BaseWeapon;
            BaseItem tempItem = player.GetComponent<Inventory>().items[0];
            player.GetComponent<Inventory>().items[0] = currentItem;
            player.GetComponent<Inventory>().items[currentPlace] = tempItem;
            player.GetComponent<Inventory>().equippedWeapon = player.GetComponent<Inventory>().items[0] as BaseWeapon;
            Debug.Log(player.GetComponent<Inventory>().equippedWeapon.name);
            GoBack();
        }
        else if (currentItem.type == BaseItem.ItemType.POTION)
        {
            if (player.curHealth < player.maxHealth)
            {
                if (currentItem.hRestore > 0)
                {
                    player.curHealth += currentItem.hRestore;
                    player.GetComponent<Inventory>().items.Remove(currentItem);
                    ActionMenu.enemiesTiles.Clear();
                    MagicMenu.enemiesTiles.Clear();
                    ItemUI.SetActive(false);
                    child.SetActive(false);
                    ActionMenu.action = false;
                    MagicMenu.action = false;
                    EquipMenu.equip = false;
                    foreach (GameObject view in ActionMenu.views)
                    {

                        Destroy(view);
                    }
                    ActionMenu.views.Clear();

                    //TacticsCam.rightRot.SetActive(true);
                    //TacticsCam.rightRot.SetActive(true);
                    player.temp = player.transform.position;
                    TurnManager.EndTurn();
                }
                else if (currentItem.mRestore > 0)
                {
                    player.curMana += currentItem.hRestore;
                    player.GetComponent<Inventory>().items.Remove(currentItem);
                    ActionMenu.enemiesTiles.Clear();
                    MagicMenu.enemiesTiles.Clear();
                    ItemUI.SetActive(false);
                    child.SetActive(false);
                    ActionMenu.action = false;
                    MagicMenu.action = false;
                    EquipMenu.equip = false;
                    foreach (GameObject view in ActionMenu.views)
                    {

                        Destroy(view);
                    }
                    ActionMenu.views.Clear();
                    //TacticsCam.rightRot.SetActive(true);
                    //TacticsCam.rightRot.SetActive(true);
                    player.temp = player.transform.position;
                    TurnManager.EndTurn();
                }
            }
            else
            {
                Debug.Log("HP is full");
            }
        }


    }
    public void GoBack()
    {
        ItemUI.SetActive(false);
        child.SetActive(false);
        EquipMenu.StartEquipUI();

        // player.velocity = new Vector3();
        //  player.heading = new Vector3();
    }
    void Update()
    {
        playerName.text = player.name;
        attack.text = player.attack.ToString();
        defense.text = player.defense.ToString();
        oldHP.maxValue = player.maxHealth;
        newHP.maxValue = player.maxHealth;
        oldMana.maxValue = newMana.maxValue = player.maxMana;
        oldHP.value = player.curHealth;
        oldMana.value = player.curMana;
        ItemIcon.sprite = currentItem.icon;
        ItemName.text = currentItem.name;
        if (currentItem.type == BaseItem.ItemType.WEAPON)
        {
            useEquip.text = "Equip";
            HPMod.text = "Damage:";
            Uses.text = "Durability:";
        }
        else if (currentItem.type == BaseItem.ItemType.POTION)
        {
            useEquip.text = "Use";
            HPMod.text = "Heals:";
            Uses.text = "Uses:";
        }
        else
        {
            useEquip.text = "(Can't Use)";
        }
    }
}
