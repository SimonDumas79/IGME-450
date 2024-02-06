using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LoadingScene : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(LoadSceneAsyc());
    }

    IEnumerator LoadSceneAsyc()
    {
        //This operation uses the sceneToLoad to load the next scene allowing this to be reused
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
        //Stop next scene from loading
        operation.allowSceneActivation = false;
        
        while (!operation.isDone)
        {
            //Here we can add whatever else we want. Tips and stuff
            

            if (operation.progress >= 0.9f)
            {

                //allow next scene to load
                //yield return new WaitForSeconds(1); // Waits for 1 second just because

                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
