using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelLoader : MonoBehaviour
{
    public void testOnclick()
    {
        string temp = gameObject.GetComponentInChildren<TMP_Text>().text;
        LoadingScene.sceneToLoad = temp;
        SceneManager.LoadScene("LoadingScene");
    }
}
