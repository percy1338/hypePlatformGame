using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [Header("Entities")]
	public EventSystem ES;
	private GameObject storeSelected;

    [Header("Controller setting")]
    public ControllerDetection controller;

    GameObject[] button;

    private void Awake()
    {
        
        //button = GameObject.FindGameObjectsWithTag("ControllerUIs");
    }

    // Use this for initialization
    void Start () {
		storeSelected = ES.firstSelectedGameObject;
    }
	
	// Update is called once per frame
	void Update () {
		if(ES.currentSelectedGameObject != storeSelected){
			if(ES.currentSelectedGameObject == null)
				ES.SetSelectedGameObject(storeSelected);
			else
				storeSelected = ES.currentSelectedGameObject;
		}

        
    }

    public void ControllerUpdate()
    {
        if (controller.Xbox_One_Controller == 0)
        {
            foreach (GameObject button in GameObject.FindGameObjectsWithTag("ControllerUIs"))
            {
                print("disabled controller icons");
                //if (button.GetComponent<Image>() != null)
                button.GetComponent<Image>().enabled = false;
            }
            //button.GetComponent<Image>().enabled = false;
        }
        else if (controller.Xbox_One_Controller == 1)
        {
            foreach (GameObject button in GameObject.FindGameObjectsWithTag("ControllerUIs"))
            {
                print("enabled controller icons");
                //if(button.GetComponent<Image>() != null)
                button.GetComponent<Image>().enabled = true;
            }
            //button.Get
            //button.GetComponent<Image>().enabled = true;
        }
    }
}
