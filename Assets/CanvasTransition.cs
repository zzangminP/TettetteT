using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasTransition : MonoBehaviour
{
    public Transform chooseblock1;
    public Transform chooseblock2;
    [SerializeField]
    public float y;
    [SerializeField]
    public float x;


    // Start is called before the first frame update
    void Start()
    {
        
        chooseblock1.DOMoveY(y, 2f);
        chooseblock2.DOMoveX(x, 2f);
    }

    IEnumerator Wait_co()
    {
        yield return new WaitForSeconds(2f);
    }

    // Update is called once per frame
}
