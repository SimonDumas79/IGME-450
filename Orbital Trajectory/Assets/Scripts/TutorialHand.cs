using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{

    enum HandState
    {
        fadeIn,
        startToMiddle,
        middle,
        middleToEnd,
        end,
        fadeOut,
        beforeStart
    }
    public PlayerController player;

    public List<Sprite> sprites;
    public Vector3 startPosition;
    public Vector3 middlePosition;
    public Vector3 endPosition;

    private float fadeTime;
    public float fadeTimeBase = 1.5f;
    private int fadeDirection = 1; //should only be 1 or -1

    [SerializeField]
    private float startToMiddleTimer;
    public float startToMiddleTimerBase = 2.0f;

    [SerializeField]
    private float middleToEndTimer;
    public float middleToEndTimerBase = 2.0f;

    private float pauseTime = 0.5f;
    public float pauseTimeBase = 0.5f;

    [SerializeField]
    private HandState state = HandState.fadeIn;

    private SpriteRenderer sr;

    public bool visible;
    protected bool fadesOut;
    private bool baseVisibility;


    public float fadeSpeedMultiplier = 1;

    virtual protected void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Color color = sr.color;
        color.a = 0;
        sr.color = color;
        baseVisibility = visible;
        fadeTime = fadeTimeBase;
        fadesOut = true;
    }
    void Update()
    {
        UpdateHand();

        if(player.Launched)
        {
            visible = !baseVisibility;
        }
    }

    protected void UpdateHand()
    {
        if (visible)
        {
            switch (state)
            {
                case HandState.fadeIn:
                    fadeTime -= Time.deltaTime;
                    ChangeAlpha(fadeDirection, fadeSpeedMultiplier);
                    if (fadeTime < 0)
                    {
                        state = HandState.startToMiddle;
                        fadeTime = fadeTimeBase;
                        fadeDirection = -1;
                    }

                    break;

                case HandState.startToMiddle:

                    startToMiddleTimer += Time.deltaTime;

                    // Calculate the interpolation factor using SmoothStep
                    float t = Mathf.SmoothStep(0f, 1f, startToMiddleTimer / startToMiddleTimerBase);

                    // Use Vector3.Lerp to interpolate between startPoint and endPoint
                    Vector3 lerpedPositionStartToMiddle = Vector3.Lerp(startPosition, middlePosition, t);

                    // Set the object's position to the lerped position
                    transform.position = lerpedPositionStartToMiddle;

                    // Reset time when it reaches or exceeds the duration
                    if (startToMiddleTimer >= startToMiddleTimerBase)
                    {
                        startToMiddleTimer = 0.0f;
                        state = HandState.middle;
                    }

                    break;

                case HandState.middle:
                    pauseTime -= Time.deltaTime;
                    if (pauseTime < pauseTimeBase / 2)
                    {
                        sr.sprite = sprites[1];
                    }
                    if (pauseTime < 0)
                    {
                        state = HandState.middleToEnd;
                        pauseTime = pauseTimeBase;
                    }
                    break;

                case HandState.middleToEnd:

                    middleToEndTimer += Time.deltaTime;

                    // Calculate the interpolation factor using SmoothStep
                    float f = Mathf.SmoothStep(0f, 1f, middleToEndTimer / middleToEndTimerBase);

                    // Use Vector3.Lerp to interpolate between startPoint and endPoint
                    Vector3 lerpedPositionMiddleToEnd = Vector3.Lerp(middlePosition, endPosition, f);

                    // Set the object's position to the lerped position
                    transform.position = lerpedPositionMiddleToEnd;

                    // Reset time when it reaches or exceeds the duration
                    if (middleToEndTimer >= middleToEndTimerBase)
                    {
                        middleToEndTimer = 0.0f;
                        state = HandState.end;
                    }

                    break;
                case HandState.end:
                    pauseTime -= Time.deltaTime;
                    if (pauseTime < pauseTimeBase / 2)
                    {
                        sr.sprite = sprites[0];
                    }
                    if (pauseTime < 0)
                    {
                        if(fadesOut)
                        {
                            state = HandState.fadeOut;
                            pauseTime = pauseTimeBase;
                        }
                        else
                        {
                            state = HandState.startToMiddle;
                        }
                    }
                    break;
                case HandState.fadeOut:
                    fadeTime -= Time.deltaTime;
                    ChangeAlpha(fadeDirection);
                    if (fadeTime < 0)
                    {
                        state = HandState.beforeStart;
                        fadeTime = fadeTimeBase;
                        fadeDirection = 1;
                        transform.position = startPosition;
                    }
                    break;
                case HandState.beforeStart:
                    pauseTime -= Time.deltaTime;

                    if (pauseTime < 0)
                    {
                        state = HandState.fadeIn;
                        pauseTime = pauseTimeBase;
                    }
                    break;
            }

        }
        else
        {
            ChangeAlpha(-1, 3.0f);
        }
    }

    private void ChangeAlpha(int direction, float speedMultiplier = 1.0f)
    {
        Color color = sr.color;
        color.a = Mathf.Clamp01(color.a + (direction * Time.deltaTime * fadeTime * speedMultiplier));
        sr.color = color;
    }
}
