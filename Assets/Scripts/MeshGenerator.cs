using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class MeshGenerator : MonoBehaviour
{


    public int dimension = 20;

    public float variation = .3f;
    public float add = 0f;
    public int actualY;

    public int maxHeight = 10;
    public int minHeight = 0;
    public float maxGroundAngle = 40f;

    public int addX;
    public int addZ;
    public int gridDim;
    public int worldDim;
    public float acceptableCornerHeight;
    public float acceptableMidHeight;

    [System.Serializable]
    public struct XCoords
    {
        [SerializeField] 
        public int[] xCoords;
    }
    public XCoords[] zCoords;

    public Terrain terrain;



    

}
