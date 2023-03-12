using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public Identity cha;
    public Inventory inventory;


    public Transform[] panels;
    public Text[] texts;

    public Color OGColor;

    public bool itemUIChange;
    public bool itemSwitch;

    void Awake()
    {
        GetInventoryUIText();

        itemUIChange = true;
    }
    void Update()
    {
        if (itemUIChange&& inventory!= null)
        {
            DisplayInventory();
        }
        else if(inventory==null)
        {
            inventory = cha.inventory; //LINE WITH ERROR
        }
        ToggleInventoryUI();
    }

    public void InventoryUIChange(Identity newChar)
    {
        inventory = newChar.inventory;
        itemUIChange = true;
    }

    void GetInventoryUIText()
    {
        panels = inventoryPanel.GetComponentsInChildren<Transform>();
        List<Text> tempTexts = new List<Text>();
        OGColor= panels[0].GetComponent<Image>().color;
        for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].name == "Text")
                {
                    Text t = panels[i].GetComponent<Text>();
                    tempTexts.Add(t);
                }

            }
        texts = new Text[tempTexts.Count];
        for (int i = 0; i < tempTexts.Count; i++)
        {
            texts[i] = tempTexts[i];
        }
        
    }

    void DisplayInventory()
    {
        
        for (int i = 0; i < texts.Length; i++)
        {
            if(inventory.maxIndex > i)
            {
                texts[i].text = inventory.items[i].description;
                if (inventory.items[i]== inventory.equippedWeapon)
                {
                    texts[i].transform.parent.GetComponent<Image>().color = Color.red;
                }
                else
                {
                    texts[i].transform.parent.GetComponent<Image>().color = OGColor;
                }
            }
            else
            {
                texts[i].text = "";
            }
        }
        
    }

    void ToggleInventoryUI()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
            }
            else
            {
                inventoryPanel.SetActive(true);
            }
        }
    }
}
