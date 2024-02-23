using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    void Start()
    {
        for(int i = 0; i < GameData.levelList.Count; i++)
        {
            GameObject makeButton = Instantiate(buttonPrefab);
            makeButton.GetComponentInChildren<TextMesh>().text = GameData.levelList[i];
            Debug.Log(makeButton.name);
        }
    }   
}
