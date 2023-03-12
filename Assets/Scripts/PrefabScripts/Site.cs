using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ScheduleActivity
{
    [Range(1, 4)]
    public int timeSlot;



}
public class Site : MonoBehaviour
{
    public GameObject building;
    public BoxCollider eventPos;
    public Transform[] waits;

    public ScheduleActivity activity;
}
