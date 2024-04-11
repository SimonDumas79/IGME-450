using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D player;

    private bool finishedDisplaying = false;

    [SerializeField]
    private GameObject endUI;

    void Start()
    {
        Time.timeScale = 1;
        endUI = FindFirstObjectByType<EndScreen>(FindObjectsInactive.Include)?.gameObject;
        if(endUI != null)
        {
            //endUI.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(endUI != null)
        {
            string levelName = SceneManager.GetActiveScene().name;

            int levelIndex = levelName.IndexOf("Level") + 5;
            int underscoreIndex = levelName.IndexOf('_');
            levelName = levelName.Substring(levelIndex, underscoreIndex - levelIndex);
            int levelNumber;
            if(!int.TryParse(levelName, out levelNumber))
            {
                Debug.LogError("Level Name Format Incorrect");
            }
            else
            {
                if(!PlayerPrefs.HasKey(GameData.levelProgressName) || PlayerPrefs.GetInt(GameData.levelProgressName) < levelNumber)
                {
                    PlayerPrefs.SetInt(GameData.levelProgressName, levelNumber);
                }
            }



            endUI.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
