using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    // Start is called before the first frame update
    /*
     GameObject buttonPrefab;

    void MyAwesomeCreator()
    {
        GameObject go = Instantiate(buttonPrefab);
        var button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => FooOnClick());
    }
    void FooOnClick()
    {
        Debug.Log("Ta-Da!");
    }
     */
    [SerializeField]
    GameObject buttonPrefab;

    [SerializeField]
    Vector2 startPos;

    [SerializeField]
    Vector2 offset;

    [SerializeField]
    int numButtonsPerRow;

    void Start()
    {
        if(GameData.levelList == null)
        {
            GameData.levelList = GameData.getAllLevels();
        }

        Canvas canvas = FindAnyObjectByType<Canvas>();
        for(int i = 0; i < GameData.levelList.Count; i++)
        {
            string fullLevelName = GameData.levelList[i];
            GameObject makeButton = Instantiate(buttonPrefab, canvas.transform);
            makeButton.GetComponent<LoadingSceneTrigger>().targetScene = fullLevelName;
            TMP_Text text = makeButton.GetComponentInChildren<TMP_Text>();
            text.text = FormatLevelName(fullLevelName);
            Debug.Log(makeButton.name);
            makeButton.transform.position = startPos + new Vector2(offset.x * (i % numButtonsPerRow), offset.y * (i/numButtonsPerRow));
        }
    }

    string FormatLevelName(string levelName)
    {
        int nameStartIndex = levelName.LastIndexOf('_') + 1;
        int fileTypeStartIndex = levelName.LastIndexOf('.');
        string shortLevelName = levelName.Substring(nameStartIndex, fileTypeStartIndex - nameStartIndex);
        string finalLevelName = "";

        int prevWordStartIndex = 0;
        for(int i = 1; i < shortLevelName.Length; i++)
        {
            if(shortLevelName[i] >= 'A' && shortLevelName[i] <= 'Z')
            {
                finalLevelName += shortLevelName.Substring(prevWordStartIndex, i - prevWordStartIndex) + " ";
                prevWordStartIndex = i;
            }
        }
        finalLevelName += shortLevelName.Substring(prevWordStartIndex);

        return finalLevelName;
    }
}