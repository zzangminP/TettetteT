using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlocks : MonoBehaviour
{

    public int[] choosedTetroIndex = new int[7];

    void Start()
    {

        for(int i = 0; i< choosedTetroIndex.Length; i++)
        {
            choosedTetroIndex[i] = i;
            Debug.Log(choosedTetroIndex[i]);
        }
        
    }

}
