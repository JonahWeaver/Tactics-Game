using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : Controller
{
    public float mVert;
    public float mHor;

    public bool interact;
    public override void ReadInput(InputData data)
    {
        RegisterInput(data);

        newInput = true;
    }

    void RegisterInput(InputData data)
    {
        if (data.axes != null)
        {
            inputData = data;

            mVert = data.axes[0];
            mHor = data.axes[1];

            interact = data.buttons[2];
        }
    }
}
