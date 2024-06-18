using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetBlockIndex : MonoBehaviour
{
    public Image img;
    public string name;
    public Transform child;
    public Transform grandChild;
    public int[] a;

    public void CheckBlockIndex()
    {
        int[] a = new int[7];

        //Debug.Log(transform.childCount);
        //img = transform.GetChild();
        for(int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            grandChild = child.GetChild(0);
            img = grandChild.GetComponent<Image>();
            name = img.sprite.name;
            a[i] = int.Parse(name) - 1;
        }

        foreach (int j in a)
        {
            Debug.Log(j);
        }

        //Debug.Log(child);
        //
        //
        //Debug.Log(img);
        //Debug.Log(img.sprite.name);
        //Debug.Log(img.sprite.name.GetType());
        //int numOfChild = this.transform.childCount;

        //for(int i = 0; i < numOfChild; i++)
        //{

        //Debug.Log(a);
        //Debug.Log(a.GetType());

        //}







        //return a;
    }
}
