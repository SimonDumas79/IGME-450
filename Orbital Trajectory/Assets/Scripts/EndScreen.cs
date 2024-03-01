using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject nextLevelButton;
    // Start is called before the first frame update
    [SerializeField]
    private TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {

    }

    private int CalculateScore()
    {
        float fuel = FindAnyObjectByType<FuelBar>().slider.value;
        return Mathf.FloorToInt(fuel * 1000.0f);
    }

}
