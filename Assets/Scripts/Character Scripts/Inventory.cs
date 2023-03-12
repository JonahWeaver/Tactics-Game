using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<BaseItem> items= new List<BaseItem>();
    public BaseWeapon equippedWeapon;
    public int equipIndex;

    bool itemChange;
    bool itemSwitch;
    [Range (0,4)]
    public int maxIndex;

    void Awake()
    {
        maxIndex = 0;
    }
    void Update()
    {
        if(itemChange)
        {
            //if a single item gets added when multiple should, or something similar, 
            //it is because of the manner of the flag check

            OrganizeItems();
        }
    }

    public void AddItem(BaseItem item)
    {
        items.Add(item);
        maxIndex++;
        itemChange = true;
          
    }

    public void RemoveItem(BaseItem item)
    {
        items.Remove(item);

        maxIndex--;
        itemChange = true;
    }

    public void SwitchItems(BaseItem item1, BaseItem item2)
    {
        //int index1 = System.Array.IndexOf(items, item1);
        //int index2 = System.Array.IndexOf(items, item2);

        //items[index1] = item2;
        //items[index2] = item1;

        //itemChange = true;
    }

    public void OrganizeItems()
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i]==null)
            { 
                for (int j = i+1; j < items.Count; j++)
                {
                    items[j - 1] = items[j];
                }
            }
        }

        itemChange = false;
    }

    public void EquipItem(BaseWeapon item, BaseCharacter cha)
    {
        if(equippedWeapon)
        {
            equippedWeapon.Unequip(cha);
        }
        equippedWeapon = item;
        equippedWeapon.Equip(cha);
    }
}
