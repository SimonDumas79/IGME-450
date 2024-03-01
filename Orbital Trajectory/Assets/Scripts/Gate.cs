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
        endUI = FindFirstObjectByType<EndScreen>(FindObjectsInactive.Include)?.gameObject;
        if(endUI != null)
        {
            endUI.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(endUI != null)
        {
            endUI.SetActive(true);
        }
    }
}
