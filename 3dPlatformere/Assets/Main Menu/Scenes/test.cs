using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{

    public Image fill;

    // Update is called once per frame
    void Update()
    {
        if (fill.fillAmount == 1)
            SceneManager.LoadScene(0);
    }
}
