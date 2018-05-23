using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    public Animator animator;
    public LevelChanger fadeToLevel;

    public void Update()
    {
        if(Input.anyKey)
            animator.SetTrigger("StartPressed");
    }

}
