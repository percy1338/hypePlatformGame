using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    public void LoadLevelNumber(int level)
    {
        StartCoroutine(LoadScene(level));
    }

    IEnumerator LoadScene(int level)
    {
        yield return new WaitForSeconds(2);
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }

}
