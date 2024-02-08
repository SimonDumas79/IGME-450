using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneTrigger : MonoBehaviour
{
    [SerializeField]
    string targetScene;

    public void LoadScene()
    {
        if(targetScene == "")
        {
            targetScene = SceneManager.GetActiveScene().name;
            LoadingData.sceneToLoad = targetScene;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            LoadingData.sceneToLoad = targetScene;
            SceneManager.LoadScene("LoadingScene");
        }
        
    }
}
