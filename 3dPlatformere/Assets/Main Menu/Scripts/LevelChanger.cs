using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour {

    public Animator animator;
    private int levelToLoad;
    public GameObject loadCanvas;
    public GameObject loadBar;
    public Text load;
    public AudioSource audSource;

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public IEnumerator OnFadeComplete()
    {
        StartCoroutine(AudioFadeOut.FadeOut(audSource, 0.3f));
        loadCanvas.SetActive(true);
        loadBar.SetActive(true);

        yield return new WaitForSeconds(3);

        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelToLoad);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            load.text = "NOW   LOADING  .  .  .";

            if (asyncOperation.progress >= 0.9f)
            {
                loadBar.SetActive(false);
                load.text = "ANY  KEY  TO  CONTINUE";
                if (Input.anyKeyDown)
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ButtonPressed(int level)
    {
        level = levelToLoad;
        FadeToLevel(level);
    }

    public void FadeToMainMenu()
    {
        FadeToLevel(0);
    }
}
