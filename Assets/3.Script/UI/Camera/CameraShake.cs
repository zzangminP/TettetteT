using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    //카메라 흔들림 구현

    public float shakeAmount;
    private float shakeTime;
    Vector3 initialPosition;


    public void ShakeForTime(float time)
    {
        shakeTime = time;

    }

   
    void Start()
    {
        initialPosition = new Vector3(0f, 0f, -5f);
        shakeAmount = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere/2 * shakeAmount + initialPosition;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            shakeTime = 0.0f;
            transform.position = initialPosition;
        }
        
    }
}
