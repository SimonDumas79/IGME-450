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
        }
        else
        {
            LoadingData.sceneToLoad = targetScene;
            SceneManager.LoadScene("LoadingScene");
        }
        
    }
}
