using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class WalkingController : Controller
{
    //movement info
    Vector3 walkVelocityX;
    Vector3 walkVelocityZ;
    public Vector3 walkVelocity;

    Vector3 prevWalkVelocityX;
    Vector3 prevWalkVelocityZ;
    Vector3 prevWalkVelocity;

    public float adjVertVelocity;
    public float jumpPressTime;

    public float maxGroundAngle = 140f;
    public float groundAngle=0f;

    public Vector3 trueForward;
    public Vector3 trueRight;

    public bool grounded=false;
    RaycastHit hitInfo;
    float height = 2f;

    public LayerMask ground;
    public Vector3 camForward;
    public Vector3 camRight;
    public Vector3 debugForward;

    public Quaternion stopRotation;
    bool stopped= true;

    //Inittialization Variables
    public int worldDim;
    //settings
    public float walkSpeed = 3f;
    public float turnSpeed = 6f;
    public float jumpSpeed = 2.5f;
    public float interactDuration = .1f;

    //delegates and events
    public delegate void HitboxEventHandler(float dur, bool act);
    public static event HitboxEventHandler OnInteract;

    public static InteractPreview ip;

    public float mVert;
    public float mHor;

    public bool jump;
    public bool test;
    public bool interact;

    public int dimension;
    public int chunkDim;
    public int radOfChunks;

    protected override void Start()
    {
        base.Start();
        //BattleInit.battleInit.GetTileCoords(worldDim);
    }
    public override void ReadInput(InputData data)
    {
        RegisterInput(data);

        camForward = new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z);
        camRight = new Vector3(mainCam.transform.right.x, 0, mainCam.transform.right.z);
        debugForward = mainCam.transform.forward;

        prevWalkVelocityX = walkVelocityX;
        prevWalkVelocityZ = walkVelocityZ;

        ResetMovement();
        //set vertical movement

        grounded=Grounded();
        CalcGroundAngle();
        CalcTrueForward();
        CalcTrueRight();

        ip = GetComponentInChildren<InteractPreview>();

        if (mVert != 0f && groundAngle <= maxGroundAngle)
        {
            newInput = true;
            if (Grounded())
            {
                walkVelocityZ = trueForward * data.axes[0];
            }
            else if (prevWalkVelocityZ != null)
            {
                walkVelocityZ = prevWalkVelocityZ;
            }
        }
        else 
        {
            walkVelocityZ = Vector3.zero;
        }


        //set horizontal movement
        if (mHor != 0f && groundAngle <= maxGroundAngle)
        {
            newInput = true;
            if (Grounded())
            {
                walkVelocityX = trueRight * data.axes[1];

            }
            else if (prevWalkVelocityX != null)
            {
                walkVelocityX = prevWalkVelocityX;
            }
        }
        else
        {
            walkVelocityX = Vector3.zero;
        }
        walkVelocity = walkVelocityX + walkVelocityZ;
        walkVelocity.Normalize();
        walkVelocity *= walkSpeed;

        prevWalkVelocity = prevWalkVelocityX + prevWalkVelocityZ;
        prevWalkVelocity.Normalize();
        prevWalkVelocity *= walkSpeed;

        //set vertical jump
        if (jump)
        {
            if (jumpPressTime == 0f && Grounded())
            {
                adjVertVelocity = jumpSpeed;
            }
            jumpPressTime += Time.deltaTime;
            newInput = true;
        }
        else
        {
            jumpPressTime = 0f;
        }
        

        //check if interact button is pressed
        
        if(test)
        {
            InitGrid();
            newInput = true;
        }

        if(OnInteract!=null)
            {
                OnInteract(interactDuration, data.buttons[1]);
            }

        if(ip.identity != null && interact)
        {
            StartConversation();
            newInput = true;
        }

    }
    
    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, out hitInfo, coll.bounds.extents.y+.15f, ground);
    }

    void CalcTrueForward()
    {
        if(!Grounded())
        {
            trueForward = camForward;
            return;
        }

        trueForward = Vector3.Cross(hitInfo.normal, -camRight);
    }
    void CalcTrueRight()
    {
        if (!Grounded())
        {
            trueRight = camRight;
            return;
        }

        trueRight = Vector3.Cross(hitInfo.normal, camForward);
    }
    void CalcGroundAngle()
    {
        if(!Grounded())
        {
            groundAngle = 0;
            return;
        }
        groundAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
    }

    void ResetMovement()
    {
        adjVertVelocity = 0f;
        walkVelocity = Vector3.zero;
    }
    void DrawDebugLines()
    {
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
        Ray mousePos= Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;
        Vector3 hitPos;
        if(Physics.Raycast(mousePos, out hit, 100, ground))
        {
            hitPos = hit.point;
            Debug.DrawLine(mousePoint, hitPos,Color.yellow) ;
        }
    }
    
    void Update()
    {
        grounded = Grounded();
        if (!newInput)
        {
            prevWalkVelocity = walkVelocity;
            ResetMovement();
            jumpPressTime = 0f;
            if(grounded && camForward!=Vector3.zero&& stopped)
            {
               stopRotation= Quaternion.LookRotation(transform.forward, Vector3.up);
               stopped = false;
            }
            if(stopRotation!= null&&Grounded())
            {
                transform.rotation = stopRotation;
            }
            Reset(inputData);
            RegisterInput(inputData);
        }
        else
        {
            float tempRotation = transform.rotation.eulerAngles.y;

            if (walkVelocity != Vector3.zero)
            {
                Quaternion look = Quaternion.LookRotation(walkVelocity, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, look, turnSpeed*Time.deltaTime);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                
            }
            else
            {
                transform.rotation = stopRotation;
            }
            stopped = true;

            camView.transform.eulerAngles -= new Vector3(0, transform.rotation.eulerAngles.y-tempRotation, 0);
        }

        rb.velocity = new Vector3(walkVelocity.x, rb.velocity.y + adjVertVelocity, walkVelocity.z);
        


        newInput = false;
    }

    void StartConversation()
    {
        mainCam.GetComponent<OverworldCameraMovement>().enabled = false;
        switcher.Switch(switcher.controllers[1]);
    }

    void InitGrid()
    { 

        BattleInit.battleInit.MakeTiles( Mathf.RoundToInt(gameObject.transform.position.x-.5f)- (dimension+1)/2, Mathf.RoundToInt(gameObject.transform.position.z - .5f) - (dimension + 1) / 2, dimension, gameObject);
        
        //.92f is offset for character height. Needs to change
        //GetComponent<PCMove>().StartBattle();


        switcher.Switch(switcher.controllers[2]);
    }

    void RegisterInput(InputData data)
    {
        if (data.axes != null)
        {
            inputData = data;

            mVert = data.axes[0];
            mHor = data.axes[1];

            jump = data.buttons[0];
            test = data.buttons[1];
            interact = data.buttons[2];
        }
    }
}
