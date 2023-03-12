using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{

    public abstract void ReadInput(InputData data);

    protected Rigidbody rb;
    protected Collider coll;
    protected bool newInput;
    public Camera mainCam;
    protected static Camera cam;

    public static ControllerSwitcher switcher;
    public GameObject camView;

    public InputData inputData;


    void Awake()
    {
        switcher = GetComponent<ControllerSwitcher>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        cam = Camera.main;
    }
    protected virtual void Start()
    {
        if (InputManager.ins.controller!= this)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Activate()
    { 
        if(InputManager.ins.controller!=this)
        {
            foreach(Controller controller in switcher.controllers)
            {
                controller.Deactivate();
            }
            InputManager.ins.controller = this;
            this.enabled = true;
            if (mainCam!= null)
            {
                mainCam.gameObject.SetActive(true);
            }
        }

    }

    public void Deactivate()
    {
        this.enabled = false;
        if (mainCam != null)
        {
            mainCam.gameObject.SetActive(false);
        }
    }

    public void Reset(InputData data)
    {
        if (data.axes != null)
        {
            for (int i = 0; i < data.axes.Length; i++)
            {
                data.axes[i] = 0f;
            }
            for (int i = 0; i < data.buttons.Length; i++)
            {
                data.buttons[i] = false;
            }
        }
    }
}
