using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCameraMovement : MonoBehaviour
{

    public float xSpeed;
    public float ySpeed;

    float xPos;
    public float yRotation;

    //yRot debug/test
    public float yRotSum;
    public bool yRotPos;

    public float speed;

    public float camHeight;

    public float maxMoveDist;
    public float minMoveDist;

    public float ring1;
    public float ring2;
    public float ring3;

    public float ring1R;
    public float ring2R;
    public float ring3R;

    float rotateHorizontal;
    float rotateVertical;

    public GameObject player;
    public GameObject camView;

    public bool beforeFirstMove;

    public Vector3 radiusConstant;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camView = player.GetComponent<WalkingController>().camView;
        beforeFirstMove = true;
        camHeight = ring1;
        yRotPos = true;
        yRotSum = (ring1+ring2)/2;
    }

    void Start()
    {
        camView.transform.eulerAngles = new Vector3(0, 0, 0);

        Vector3 offset = player.transform.up * yRotSum;
        offset += -camView.transform.forward * GetRadius(yRotSum);
        transform.position = player.transform.position + offset;
        transform.LookAt(camView.transform);
    }
    void Update()
    {
        rotateHorizontal = Input.GetAxis("Mouse X")* xSpeed * Time.deltaTime;
        rotateVertical = Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        //sentinel code to default camera position
        //if (beforeFirstMove && rotateHorizontal == 0 && rotateVertical==0&& bRing1!=null)
        //{
        //    transform.position = bRing1;
        //    transform.LookAt(camView.transform);
        //}
        //else if(rotateHorizontal != 0 && rotateVertical != 0 && beforeFirstMove)
        //{
        //    beforeFirstMove = false;
        //}
        

        if(yRotation>0&& rotateVertical<0 || yRotation<0 && rotateVertical>0|| rotateVertical==0)
        {
            yRotation = 0;
        }
        if (rotateVertical != 0|| rotateHorizontal!=0)
        {
            yRotation -= rotateVertical;
            yRotSum += yRotation;
            yRotSum = Mathf.Clamp(yRotSum, ring3, ring1);

            Vector3 offset = player.transform.up * yRotSum;

            offset += -camView.transform.forward * GetRadius(yRotSum);

            camView.transform.localPosition = new Vector3(0, .91f, 0);

            camView.transform.RotateAround(camView.transform.position, player.transform.up, rotateHorizontal);
            camView.transform.localEulerAngles = new Vector3(0, camView.transform.localEulerAngles.y, 0);

            transform.position = player.transform.position + offset;
            transform.LookAt(camView.transform);
        }


        

        
    }

    float GetRadius(float y)
    {
        float trueRadius;
        if(y>ring2)
        {
            trueRadius = CalculateRadius(y, ring1, ring2, ring1R, ring2R);
        }
        else if(y>ring3)
        {
            trueRadius = CalculateRadius(y, ring3, ring2, ring3R, ring2R);
        }
        else if(y>=ring1)
        {
            trueRadius = ring1R;
        }
        else
        {
            trueRadius = ring3R;

        }
        return trueRadius;
    }

    float CalculateRadius(float y, float firstRing, float lastRing, float firstRingR, float lastRingR)
    {
        float percentDif = (y - (lastRing)) / (firstRing - lastRing);
        

        float addRad = (firstRingR - lastRingR) * percentDif;



        return lastRingR + addRad;
    }


}
