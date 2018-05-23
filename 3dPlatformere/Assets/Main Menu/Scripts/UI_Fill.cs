using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Fill : MonoBehaviour
{
    public Image bar;
    public float fill = 0;
    public int level;
    public LevelChanger fade;

    public void Update()
    {
        float amount = fill / 100;
        bar.fillAmount = amount;


        if (Input.GetButton("Interact"))
            fill += 100f * Time.deltaTime;
        else if (Input.GetButtonUp("Interact"))
            fill = 0;

        if (bar.fillAmount == 1)
        {
            print("Loading level...");
            fill = 100;
            fade.FadeToLevel(level);
        }
    }

    public void LoadLevel()
    {
        //SceneManager.LoadScene(level);
        fade.FadeToLevel(level);
    }
}