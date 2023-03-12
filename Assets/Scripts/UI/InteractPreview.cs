using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class InteractPreview : MonoBehaviour
{
    //collider positions
    public BoxCollider col;
    public GameObject preview;
    public Text text1;
    public Collider other;

    public Identity identity;

    //collider duration
    float duration;
    public float distance;
    public float maxDistance=1f;
    public bool active;
    public bool status;

    public WalkingController wc;
    public ConversationController cc;

    void Awake()
    {
        WalkingController.OnInteract += StartCollisionCheck;
        col = GetComponent<BoxCollider>();
        wc = transform.parent.GetComponent<WalkingController>();
        cc = transform.parent.GetComponent< ConversationController > ();
    }
    void Start()
    {
        preview.SetActive(false);
        status = false;
    }
    void Update()
    {
        if (other != null)
        {
            Vector3 closestPoint = other.ClosestPoint(transform.TransformPoint(col.center));
            distance = Vector3.Distance(closestPoint, transform.TransformPoint(col.center));
            if(Mathf.Abs(distance)>maxDistance)
            {
                preview.SetActive(false);
                other = null;
            }
            else if(active)
            {
                preview.SetActive(false);
            }
            else
            {
                preview.SetActive(true);
                text1.text = other.name;
                identity = other.GetComponent<Identity>();
            }
        }
        else
        {
            preview.SetActive(false);
            identity = null;
        }
        
    }

    void StartCollisionCheck(float dur, bool act)
    {
        duration = dur;
        active = act;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!active)
        {
            preview.SetActive(true);
        }
        this.other = other;
    }
}
