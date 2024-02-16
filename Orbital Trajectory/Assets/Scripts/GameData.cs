using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameData
{
    static Scene LevelSelect;
    string[] levelArray;
    public string[] getAllLevels()
    {
        Scene scenes = SceneManager.GetSceneByPath("Assets/Scenes/Completed_Levels");

        string[] a = { "a"};
        return a;
    }
    public static void LoadLevelSelect()
    {

    }
}

