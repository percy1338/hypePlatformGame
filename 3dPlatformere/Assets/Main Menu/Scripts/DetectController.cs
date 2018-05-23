using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectController : MonoBehaviour
{
    public int Xbox_One_Controller = 0;
    public Text controllerText;
    public GUIManager controllerGUI;

    void Start()
    {
        controllerText.text = ("No controller detected");
    }

    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            //print(names[x].Length);
            if (names[x].Length == 33)
            {
                Xbox_One_Controller = 1;
            }
            else if (names[x].Length != 33)
            {
                Xbox_One_Controller = 0;
            }
        }

        Detection();
    }

    public void Detection()
    {
        if (Xbox_One_Controller == 1)
        {
            controllerText.text = ("Xbox controller detected");
        }
        else
        {
            controllerText.text = ("No controller detected");
        }
    }
}
