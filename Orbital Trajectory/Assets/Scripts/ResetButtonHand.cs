using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonHand : TutorialHand
{
    public float timeToStart;

    private void Awake()
    {
        GameObject restartButton = GameObject.Find("Restart_Button");
        transform.position = startPosition = middlePosition = endPosition =
            restartButton.transform.position;// + new Vector3(30 / transform.lossyScale.x, -16 / transform.lossyScale.y, 0);
    }

    protected override void Start()
    {
        base.Start();
        fadesOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.Launched)
        {
            timeToStart -= Time.deltaTime;
        }
        if (timeToStart < 0)
        {
            UpdateHand();
        }
    }
}
