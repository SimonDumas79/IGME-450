using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    public void testOnclick()
    {
        string temp = gameObject.GetComponentInChildren<TextMesh>().text;
        LoadingData.sceneToLoad = temp;
        SceneManager.LoadScene("LoadingScene");
    }
}
