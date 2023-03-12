using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DateAndTime
{
    [Range(1, 31)]
    public int day;
    public int dayMax;

    [Range(1, 12)]
    public int month;
    public string monthName;

    public int year;

    [Range(1, 4)]
    public int timeSlot;

    
}
public class Calendar : MonoBehaviour
{
    public static Calendar calendar { get; set; }
    public bool monthChange;
    public DateAndTime date;

    public GameObject panel;

    public Text timeText;
    public Text dateText;
    public Text weekdayText;

    public string[] weekdays;
    public int dayIndex;

    public string[] time;
    public int timeIndex;


    private void Awake()
    {
        if (calendar== null)
        {
            calendar = this;
        }
        date.day = 2;
        date.dayMax = 28;
        date.month = 2;
        date.monthName = "February";

        weekdays = new string[7];

        weekdays[0] = "Sunday";
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";

        dayIndex = 0;

        time = new string[4];

        time[0] = "Morning";
        time[1] = "Afternoon";
        time[2] = "Evening";
        time[3] = "Bedtime";

        timeIndex = 0;
    }

    void Update()
    {
        if(monthChange)
        {
            if(date.month==12)
            {
                date.month = 1;
            }
            else
            {
                date.month++;
            }

            switch(date.month)
            {
                case 1:
                    {
                        date.dayMax = 31;
                        date.monthName = "January";
                        break;
                    }
                case 2:
                    {
                        date.dayMax = 28;
                        date.monthName = "February";
                        break;
                    }
                case 3:
                    {
                        date.dayMax = 31;
                        date.monthName = "March";
                        break;
                    }
                case 4:
                    {
                        date.dayMax = 30;
                        date.monthName = "April";
                        break;
                    }
                case 5:
                    {
                        date.dayMax = 31;
                        date.monthName = "May";
                        break;
                    }
                case 6:
                    {
                        date.dayMax = 30;
                        date.monthName = "June";
                        break;
                    }
                case 7:
                    {
                        date.dayMax = 31;
                        date.monthName = "July";
                        break;
                    }
                case 8:
                    {
                        date.dayMax = 31;
                        date.monthName = "August";
                        break;
                    }
                case 9:
                    {
                        date.dayMax = 30;
                        date.monthName = "September";
                        break;
                    }
                case 10:
                    {
                        date.dayMax = 31;
                        date.monthName = "October";
                        break;
                    }
                case 11:
                    {
                        date.dayMax = 30;
                        date.monthName = "November";
                        break;
                    }
                case 12:
                    {
                        date.dayMax = 31;
                        date.monthName = "December";
                        break;
                    }
                default:
                    {
                        date.dayMax = 27;
                        date.monthName = "Error";
                        break;
                    }
            }
            monthChange = false;
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                if(timeIndex<3)
                {
                    timeIndex++;
                }
                else
                {
                    timeIndex = 0;
                    DayProgress();
                }
                
            }
        }

        timeText.text = time[timeIndex];
        dateText.text = date.month + "/" + date.day;
        weekdayText.text = weekdays[dayIndex];

        ToggleCalendar();

    }

    void DayProgress()
    {
        if (date.day < date.dayMax)
        {
            date.day++;
        }
        else
        {
            date.day = 1;
            monthChange = true;
        }

        if (dayIndex < 6)
        {
            dayIndex++;
        }
        else
        {
            dayIndex = 0;
        }
    }
    
    void ToggleCalendar()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
            else
            {
                panel.SetActive(true);
            }
        }
    }
}
