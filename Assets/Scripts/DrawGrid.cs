using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{
    private int height ;
    private int width ;
    private string[,] dgArray;
    public DrawGrid(int height, int width)
    {
        this.height = height;
        this.width = width;

        dgArray = new string[width, height];

        for (int x=0; x< dgArray.GetLength(0); x++)
        {
            for (int y=0; y< dgArray.GetLength(1); y++)
            {
                Debug.Log(x + ", " + y);
            }
        }
    }
}
