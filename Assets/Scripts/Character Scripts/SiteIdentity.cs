using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteIdentity : MonoBehaviour
{
    public Site site;
    public int siteID;

    public int curWaitNum;


    void Awake()
    {
        site = GetComponent<Site>();
        site.waits = GetCompNoRoot<Transform>(gameObject);
        curWaitNum = 0;
    }

    T[] GetCompNoRoot<T>(GameObject obj) where T : Component
    {
        List<T> waitList = new List<T>();
        foreach (Transform child in obj.transform)
        {
            T wait = child.GetComponent<T>();
            if (wait != null)
            {
                waitList.Add(wait);
            }
        }
        return waitList.ToArray();
    }
}