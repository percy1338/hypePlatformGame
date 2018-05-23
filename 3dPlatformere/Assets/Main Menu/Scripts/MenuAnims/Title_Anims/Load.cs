//This script lets you load a Scene asynchronously. It uses an asyncOperation to calculate the progress and outputs the current progress to Text (could also be used to make progress bars).

//Attach this script to a GameObject
//Create a Button (Create>UI>Button) and a Text GameObject (Create>UI>Text) and attach them both to the Inspector of your GameObject
//In Play Mode, press your Button to load the Scene, and the Text changes depending on progress. Press the space key to activate the Scene.
//Note: The progress may look like it goes straight to 100% if your Scene doesn’t have a lot to load.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    //public Text m_Text;
    //public Button m_Button;

    void StartLoadingScene()
    {
        //Start loading the Scene asynchronously and output the progress bar
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {


            if (asyncOperation.progress >= 0.9f)
            {
                //m_Text.text = "ANY KEY TO CONTINUE";
                if (Input.anyKeyDown)
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
