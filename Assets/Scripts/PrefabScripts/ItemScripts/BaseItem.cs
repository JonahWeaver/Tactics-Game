using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    public enum ItemType
    {
        POTION,
        ENHANCER,
        WEAPON,
        ARMOR
    }
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Sprite icon;
    public int hRestore;
    public int mRestore;
}
