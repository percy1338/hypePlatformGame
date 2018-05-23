using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerUIs : MonoBehaviour
{

    DetectController controller;
    EventSystem eventSystem;
    public GameObject button;

    // Use this for initialization
    void Start()
    {
        controller = gameObject.GetComponent<DetectController>();
        //eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    public void ActivateControllerUIs()
    {
        if (controller.Xbox_One_Controller == 0)
            button.GetComponent<Image>().enabled = false;
        else if (controller.Xbox_One_Controller == 1)
            button.GetComponent<Image>().enabled = true;
    }
}
