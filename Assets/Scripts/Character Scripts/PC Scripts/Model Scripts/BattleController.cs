using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : Controller
{
    //Input Variables
    public float mVert;
    public float mHor;

    public bool test;
    public bool rotLeft;
    public bool rotRight;

    bool selAtt;
    public bool selected;
    public Tile moveTile;

    //Initialization variables
    public float yDist;
    public float zDist;
    public bool flag1=false;

    //Camera Rotation
    public bool rotating;
    public int rotation;

    public float rotationAccel;
    public float minRotationSpeed;
    public float currentRotationSpeed;
    public float maxRotationSpeed;

    static float circConstant = Mathf.PI/2;
    public float angle;
    public float accelAngle;
    public bool sentinel2;

    public float heightDif;
    public float cursorHeight = 1f;
    public float cornerDist = 100f;
    float newHeight;

    GameObject tet1;
    GameObject tet2;

    public float haltDecel;

    //Tile/Unit Selection

    public Tile previousTile;
    public Tile previousTile2;

    public bool turn;

    public float speed;
    public int dim;

    public static TacticsMove tm;

    public GameObject cursor;
    GameObject cursorSupp;
    public GameObject world;

    public override void ReadInput(InputData data)
    {
        RegisterInput(data);
        
        newInput = true;
    }

    protected override void Start()
    {
        base.Start();
        enabled = false;
        rotation = 0;
        angle = 0;
        accelAngle = 0;
        tet1 = new GameObject();
        tet1.name = "tet1";
        tet2 = new GameObject();
        tet2.name = "tet2";
        cursorSupp = new GameObject();
        cursorSupp.name = "Supplement";
        currentRotationSpeed = minRotationSpeed;
    }


    void Update()
    {
        

        if(!flag1)
        {
            BattleControllerInit();
            //ColorTiles();
            dim = GetComponent<WalkingController>().dimension;
            flag1 = true;

        }
        if(newInput&& !rotating&& TurnManager.PlayerTurn&&!selected)
        {
            //mVert && mHor
            if(!selected && ActionMenu.action)
            {
                return;
            }
            if (mVert != 0 || mHor != 0)
            {
                cursor.transform.position += speed * Time.deltaTime * (mHor*Vector3.Normalize(new Vector3(mainCam.transform.right.x, 0, mainCam.transform.right.z)) + mVert*Vector3.Normalize(new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z)));
                mainCam.transform.position += speed * Time.deltaTime * (mHor * Vector3.Normalize(new Vector3(mainCam.transform.right.x, 0, mainCam.transform.right.z)) + mVert * Vector3.Normalize(new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z)));
                //cursor.transform.position = new Vector3(cursor.transform.position.x, heightDif + cursorHeight + .42f, cursor.transform.position.z);
                //mainCam.transform.position = new Vector3(mainCam.transform.position.x, heightDif+ yDist, mainCam.transform.position.z);
            }
            //rotLeft && rotRight
            TrackRotation();
            if (selAtt)
            {
                RaycastHit hit;
                if (Physics.Raycast(cursor.transform.position, cursor.transform.up * -1.9f, out hit))
                {
                    Tile t = hit.collider.GetComponent<Tile>();
                    if(t.selectable)
                    {
                        selected = true;
                    }
                }
            }
            

            //miscellaneous
            ColorCursorTile();
            Reset(inputData);
        }
        else if(rotating&&TurnManager.PlayerTurn)
        { 
            if(newInput)
            {
                Reset(inputData);
            }

            CameraRotate();
        }

        //if (test)
        //{
        //    gameObject.GetComponent<Rigidbody>().useGravity = true;
        //    switcher.Switch(switcher.controllers[0]);
        //}

        newInput = false;
    }

    void RegisterInput(InputData data)
    {
        if (data.axes != null)
        {
            inputData = data;

            mVert = data.axes[0];
            mHor = data.axes[1];

            selAtt = data.buttons[1];
            rotLeft = data.buttons[3];
            rotRight = data.buttons[4];
        }
    }

    void ColorTiles()
    {
        foreach(GameObject toColor in GameObject.FindGameObjectsWithTag("Tiles"))
        {
            Tile tC = toColor.GetComponent<Tile>();
            tC.colorChanged = true;
        }
    }

    void ColorCursorTile()
    {
        RaycastHit hit;

        if (Physics.Raycast(cursor.transform.position,cursor.transform.up* -1.9f, out hit))
        {
            if (hit.collider.tag == "Tiles")
            { 
                Tile t = hit.collider.GetComponent<Tile>();
                if (t!=previousTile)
                {
                    if (previousTile != null)
                    {
                        previousTile.cursor = false;
                        previousTile.colorChanged = true;
                    }
                    previousTile = t;
                    t.cursor = true;
                    t.colorChanged = true;
                    heightDif = Mathf.RoundToInt((-cursor.transform.position.y+(t.transform.position.y+1.92f))*2)/2f;
                    cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y+heightDif, cursor.transform.position.z);
                    mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + heightDif, mainCam.transform.position.z);
                    moveTile = t;
                }
            }
        }
        else if(Physics.Raycast(cursor.transform.position-cursor.transform.up, cursor.transform.up * 2.9f, out hit))
        {
            if (hit.collider.tag == "Tiles")
            {
                Tile t = hit.collider.GetComponent<Tile>();
                if (t != previousTile)
                {
                    if (previousTile != null)
                    {
                        previousTile.cursor = false;
                        previousTile.colorChanged = true;
                    }
                    previousTile = t;
                    t.cursor = true;
                    t.colorChanged = true;
                    heightDif = Mathf.RoundToInt((-cursor.transform.position.y + (t.transform.position.y + 1.92f)) * 2) / 2f;
                    cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y + heightDif, cursor.transform.position.z);
                    mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + heightDif, mainCam.transform.position.z);
                }
            }
        }


    }

    void ChangingElevation()
    {
        RaycastHit hit;
        if (Physics.Raycast(cursor.transform.position+new Vector3(0,16,0), -cursor.transform.up, out hit, 32))
        {
            if (hit.transform.tag == "Tiles")
            {
                heightDif = hit.transform.position.y+.5f;
            }
        }
    }

    void CameraRotate()
    {
        if (rotation>0)
        {
            if(angle>(-(circConstant))-accelAngle)
            {
                currentRotationSpeed = Accelerate(currentRotationSpeed, maxRotationSpeed, rotationAccel);
                angle -= Time.deltaTime * currentRotationSpeed;
            }
            else
            {
                currentRotationSpeed = Decelerate(currentRotationSpeed, rotationAccel);
                angle -= Time.deltaTime * currentRotationSpeed;
            }
        }
        else
        {
            if (angle <= circConstant - accelAngle)
            {
                currentRotationSpeed = Accelerate(currentRotationSpeed, maxRotationSpeed, rotationAccel);
                angle += Time.deltaTime * currentRotationSpeed;
            }
            else
            {
                currentRotationSpeed = Decelerate(currentRotationSpeed, rotationAccel);
                angle += Time.deltaTime * currentRotationSpeed;
            }
        }

        float localXPos = zDist * Mathf.Sin(angle);
        float localZPos = zDist - (zDist * Mathf.Cos(angle));

        Vector3 xDirection = Vector3.Normalize(new Vector3(tet1.transform.right.x, 0, tet1.transform.right.z));
        Vector3 zDirection = Vector3.Normalize(new Vector3(tet1.transform.forward.x, 0, tet1.transform.forward.z));

        if (-angle <= circConstant && rotation > 0|| -angle > (-(circConstant)) && rotation < 0)
        {
            mainCam.transform.position = tet1.transform.position+ localZPos*zDirection + localXPos * xDirection;
            mainCam.transform.LookAt(cursorSupp.transform);
        }
        else
        {
            mainCam.transform.position = tet2.transform.position;
            mainCam.transform.rotation = tet2.transform.rotation;

            angle = 0;
            accelAngle = 0;
            currentRotationSpeed = minRotationSpeed;
            rotating = false;
            sentinel2 = false;
        }
    }

    void BattleControllerInit()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;

        mainCam.transform.position = transform.position - transform.forward * zDist + transform.up * yDist;
        mainCam.transform.LookAt(transform);
        cursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cursor.transform.localScale *= .2f;
        cursor.transform.position = gameObject.transform.position + new Vector3(0, cursorHeight, 0);
        cursorSupp.transform.position = gameObject.transform.position;
        cursorSupp.transform.parent = cursor.transform;

    }

    void TrackRotation()
    {
        if (rotRight)
        {
            rotation = -90;
            rotating = true;
            tet1.transform.position = mainCam.transform.position;
            tet1.transform.rotation = mainCam.transform.rotation;
            tet2.transform.position = mainCam.transform.position;
            tet2.transform.rotation = mainCam.transform.rotation;

            tet2.transform.RotateAround(cursor.transform.position, Vector3.up, -90);
        }
        else if (rotLeft)
        {
            rotation = 90;
            rotating = true;
            tet1.transform.position = mainCam.transform.position;
            tet1.transform.rotation = mainCam.transform.rotation;
            tet2.transform.position = mainCam.transform.position;
            tet2.transform.rotation = mainCam.transform.rotation;

            tet2.transform.RotateAround(cursor.transform.position, Vector3.up, 90);
        }
    }
    float Accelerate(float speed, float maxSpeed, float acceleration)
    {
        if(speed>=maxSpeed)
        {
            return maxSpeed;
        }
        else
        {
            speed += acceleration;
            if (speed >= maxSpeed|| angle> circConstant/2)
            {
                accelAngle = angle;
            }
            
            return speed;
        }
    }

    float Decelerate(float speed, float acceleration)
    {
        if (speed <= 0)
        {
            return 0;
        }
        else
        {
            speed -= acceleration*haltDecel;
            return speed;
        }
    }
}
