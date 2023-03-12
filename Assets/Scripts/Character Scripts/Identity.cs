using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identity : MonoBehaviour
{
    public Calendar calendar;
    public GameObject timePanel;

    public BaseCharacter character;
    public int characterID;

    public SiteIdentity dailySite;

    public string day;

    public Inventory inventory;
    public BaseItem[] defaultItems;

    string siteChoice1;
    string siteChoice2;
    string siteChoice3;
    string siteChoice4;
    string siteChoice5;
    string siteChoice6;
    string siteChoice7;

    public SiteIdentity site1;
    public SiteIdentity site2;
    public SiteIdentity site3;
    public SiteIdentity site4;
    public SiteIdentity site5;
    public SiteIdentity site6;
    public SiteIdentity site7;

    public int indexChoice1;
    public int indexChoice2;
    public int indexChoice3;
    public int indexChoice4;
    public int indexChoice5;
    public int indexChoice6;
    public int indexChoice7;

    private void Awake()
    {
        
        gameObject.AddComponent<Inventory>();
        inventory = GetComponent<Inventory>();
        bool noEquip = true;
        foreach(BaseItem item in defaultItems)
        {
            inventory.AddItem(item);
            if (noEquip && item.type == BaseItem.ItemType.WEAPON)
            {
                inventory.EquipItem(item as BaseWeapon, character);
                noEquip = false;
            }
        }
        
    }
    void Start()
    {
        calendar = GameObject.FindGameObjectsWithTag("Calendar")[0].GetComponent<Calendar>();
        if (gameObject.tag != "Player" && gameObject.tag!= "NPC")//NPC thing is temporary
        {
            siteChoice1 = site1.name;
            siteChoice2 = site2.name;
            siteChoice3 = site3.name;
            siteChoice4 = site4.name;
            siteChoice5 = site5.name;
            siteChoice6 = site6.name;
            siteChoice7 = site7.name;
        }

        timePanel = calendar.GetComponentsInChildren<Transform>()[0].gameObject;
    }
    void Update()
    {
        if (gameObject.tag != "Player" && gameObject.tag != "NPC")//NPC thing is temporary
        {
            GetSiteSchedule();
        }
    }

    void GetSite(string siteName)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Sites"))
        {
            if (obj.name == siteName)
            {
                dailySite = obj.GetComponent<SiteIdentity>();
            }
        }
    }

    void MoveToWaitSpot(int index)
    {
        gameObject.transform.position = dailySite.site.waits[index].position+ new Vector3(0, gameObject.transform.position.y,0);
        gameObject.transform.rotation = dailySite.site.waits[index].rotation;
    }

    void GetSiteSchedule()
    {
        day = calendar.weekdays[calendar.dayIndex];

        switch (day)
        {
            case "Sunday":
                {
                    GetSite(siteChoice1);
                    MoveToWaitSpot(indexChoice1);
                    break;
                }
            case "Monday":
                {
                    GetSite(siteChoice1);
                    MoveToWaitSpot(indexChoice2);
                    break;
                }
            case "Tuesday":
                {
                    GetSite(siteChoice2);
                    MoveToWaitSpot(indexChoice2);
                    break;
                }
            case "Wednesday":
                {
                    GetSite(siteChoice3);
                    MoveToWaitSpot(indexChoice3);
                    break;
                }
            case "Thursday":
                {
                    GetSite(siteChoice4);
                    MoveToWaitSpot(indexChoice4);
                    break;
                }
            case "Friday":
                {
                    GetSite(siteChoice5);
                    MoveToWaitSpot(indexChoice5);
                    break;
                }
            case "Saturday":
                {
                    GetSite(siteChoice6);
                    MoveToWaitSpot(indexChoice6);
                    break;
                }
            default:
                {
                    GetSite(siteChoice7);
                    MoveToWaitSpot(indexChoice7);
                    break;
                }
        }
    }
}
