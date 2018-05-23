using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public Image fill;

    public void ToLevel(int level)
    {
        if (fill.fillAmount == 1)
            SceneManager.LoadScene(level);
    }
}
