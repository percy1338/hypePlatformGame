using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingControl : MonoBehaviour {

    public bool leftAvailable;
    public bool rightAvailable;
    public GameObject Left;
    public GameObject Right;
    public GameObject l_ES;
    public GameObject r_ES;
    public GameObject curPos;
    public GameObject curES;

	void Update () {
        if(leftAvailable == true)
        {
            if (Input.GetButtonUp("Left"))
            {
                Left.SetActive(true);
                l_ES.SetActive(true);
                curPos.SetActive(false);
                curES.SetActive(false);
            }
        }

        if (rightAvailable == true)
        {
            if (Input.GetButtonUp("Right"))
            {
                curPos.SetActive(false);
                curES.SetActive(false);
                Right.SetActive(true);
                r_ES.SetActive(true);
            }
        }
    }
}
