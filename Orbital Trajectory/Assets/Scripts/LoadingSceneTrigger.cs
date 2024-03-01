using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneTrigger : MonoBehaviour
{
    public string targetScene;

    public void LoadScene()
    {
        if(targetScene == "")
        {
            targetScene = SceneManager.GetActiveScene().name;
            LoadingScene.sceneToLoad = targetScene;
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            LoadingScene.sceneToLoad = targetScene;
            SceneManager.LoadScene("LoadingScene");
        }

    }
    public void LoadLevelSelect()
    {
        GameData.LoadLevelSelect();
    }
}
