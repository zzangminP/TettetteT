using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    SpriteRenderer bgcolor;
    // Start is called before the first frame update
    void Start()
    {
        bgcolor = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        bgcolor.color = new Color(Mathf.Sin(Time.time), Mathf.Cos(Time.time), Mathf.Sin(Time.time / 2));
    }


}
