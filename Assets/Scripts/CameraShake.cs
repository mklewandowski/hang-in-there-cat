using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 startPosition;
    float shakeTimer = 0f;
    float shakeTimerMax = .6f;

    void Start ()
    {
        startPosition = transform.position;
    }

    void Update ()
    {
        Vector3 newPos;
        newPos = startPosition;
        if (shakeTimer > 0)
        {
            shakeTimer = shakeTimer - Time.deltaTime;
            newPos.x =  newPos.x + Random.Range(shakeTimer * -.15f, shakeTimer * .15f);
            newPos.y =  newPos.y + Random.Range(shakeTimer * -.15f, shakeTimer * .15f);
        }
        transform.position = newPos;
    }

    public void StartShake()
    {
        shakeTimer = shakeTimerMax;
    }
}
