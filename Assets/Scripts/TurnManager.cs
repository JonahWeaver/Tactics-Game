using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();
    static Queue<TacticsMove> turnTeam2 = new Queue<TacticsMove>();
    static List<TacticsMove> teamList = new List<TacticsMove>();
    //static Queue<TacticsMove> starts = new Queue<TacticsMove>();

    static int turns;
    static int turnNum;

    public static bool PlayerTurn;


    void Start()
    {
        turnNum = 1;
        turns = 0;
        PlayerTurn = false;

        //GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
        //GameObject[] eunits = GameObject.FindGameObjectsWithTag("NPC");
        ///foreach (GameObject unit in units)
        //{
        //    if (unit.name != "Cur")
        //    {
        //        TacticsMove to = unit.GetComponent<TacticsMove>();
        //        starts.Enqueue(to);
        //    }

        //}

    }


    void Update()
    {
        if (turnTeam2.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue()
    {
        teamList = new List<TacticsMove>();
        foreach (TacticsMove unit in turnTeam)
        {
            teamList.Add(unit);
        }

        SpeedSorted(teamList);

        foreach (TacticsMove unit in teamList)
        {
            if (unit != null)
            {
                if (turnNum == 1 || unit.name != "Cur")
                {
                    turnTeam2.Enqueue(unit);
                }
            }
        }
        turns = turnTeam2.Count;
        StartTurn();
    }

    static void StartTurn()
    {
        if (turnTeam2.Count > 0)
        {
            turnTeam2.Peek().BeginTurn();
        }
    }
    public static void EndTurn()
    {
        TacticsMove unit = turnTeam2.Dequeue();

        turns--;

        if (turns == 0)
        {
            turnNum++;
        }

        unit.EndTurn();

        if (turnTeam2.Count > 0)
        {
            StartTurn();
        }
        else
        {
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(TacticsMove unit)
    {
        if (turnNum == 1 || unit.tag != "Cur")
        {
            turnTeam.Enqueue(unit);
        }
    }

    public static void SpeedSorted(List<TacticsMove> units)
    {
        List<TacticsMove> sorted = new List<TacticsMove>();
        List<TacticsMove> notSorted = new List<TacticsMove>();
        int high = 0;
        TacticsMove it = null;
        foreach (TacticsMove unit in units)
        {
            notSorted.Add(unit);
        }
        while (notSorted.Count > 0)
        {
            foreach (TacticsMove unit in notSorted)
            {
                if (unit.speed > high)
                {
                    high = unit.speed;
                    it = unit;
                }
            }
            sorted.Add(it);
            notSorted.Remove(it);
            high = 0;
            it = null;
        }
        for (int i = 0; i < units.Count; i++)
        {
            units[i] = sorted[i];
        }

    }
    public static int ReturnTurnNum()
    {
        int num = turnNum;
        return num;
    }
    public static void Remove()
    {
        TacticsMove trash = turnTeam2.Dequeue();
    }
    public static TacticsMove GetActivePlayer()
    {
        TacticsMove player = new TacticsMove();
        foreach (TacticsMove unit in turnTeam2)
        {
            if (unit.turn == true)
            {
                player = unit;
            }
        }
        return player;
    }
    public static void Kill(TacticsMove unit)
    {
        turnTeam = new Queue<TacticsMove>(turnTeam.Where(p => p != unit));
        Destroy(unit.gameObject);
    }
}
