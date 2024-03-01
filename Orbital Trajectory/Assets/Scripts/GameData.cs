using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameData
{
    static Scene LevelSelect;
    public static List<string> levelList;
    public static List<string> getAllLevels()
    {
        
        int counter = 0;

        
        List<string> sceneString = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if(SceneUtility.GetScenePathByBuildIndex(i).Contains("Level" + (counter + 1)+"_"))
            {
                sceneString.Add(SceneUtility.GetScenePathByBuildIndex(i));
                Debug.Log(SceneUtility.GetScenePathByBuildIndex(i));
                counter++;
            }
        }
        return sceneString;
    }

    public static void LoadLevelSelect()
    {
        levelList = getAllLevels();
        LoadingScene.sceneToLoad = "LevelSelect";
        SceneManager.LoadScene("LoadingScene");
    }
}
