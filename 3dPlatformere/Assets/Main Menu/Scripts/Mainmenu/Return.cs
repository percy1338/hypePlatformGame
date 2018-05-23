using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour {

    public GameObject previousTab;
    public GameObject currentTab;
    public GameObject previousES;
    public GameObject currentES;

    public void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            previousTab.SetActive(true);
            currentTab.SetActive(false);

            previousES.SetActive(true);
            currentES.SetActive(false);
        }
    }

    public void GoBack()
    {
            previousTab.SetActive(true);
            currentTab.SetActive(false);

            previousES.SetActive(true);
            currentES.SetActive(false);
    }

}
