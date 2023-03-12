using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Classification
{
    PC,
    FNPC,
    NPC,
    Enemy
}

[System.Serializable]
public struct PersonalSchedule
{
    public Site[] locations;
    public DateAndTime[] dates;
    public int[] weekdays;
    

}
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class BaseCharacter : ScriptableObject
{
    public Classification classification;
    public PersonalSchedule schedule;

    public CharacterStat Health;
    public CharacterStat Mana;
    public CharacterStat Strength;
    public CharacterStat Intelligence;
    public CharacterStat Skill;
    public CharacterStat Defense;
    public CharacterStat Will;
    public CharacterStat Speed;
    public CharacterStat Attack;
    public CharacterStat Move;
    public CharacterStat JumpHeight;
    public bool bMagic;
    public bool wMagic;
}
