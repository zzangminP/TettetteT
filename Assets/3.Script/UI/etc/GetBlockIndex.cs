using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetBlockIndex : MonoBehaviour
{
    public Image img;
    [HideInInspector]
    public string name;
    [HideInInspector]
    public Transform child;
    [HideInInspector]
    public Transform grandChild;
    public int[] a;
    public GameObject text;
    
    private Transform pb;
    private PlayerBlocks pbs;
    public void CheckBlockIndex()
    {
        //text = GameObject.Find("CTText");
        //int[] a = new int[7];
        //pb = 

        //pb = GameManager.instance.transform.Find("PlayerBlocks");
        //pbs = pb.GetComponent<PlayerBlocks>();
        try
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                child = transform.GetChild(i);
                grandChild = child.GetChild(0);
                img = grandChild.GetComponent<Image>();
                name = img.sprite.name;
                //a[i] = int.Parse(name) - 1;
                GameManager.instance.choosedTetroIndex[i] = int.Parse(name) - 1;
            }

            //foreach (int j in a)
            //{
            //    Debug.Log(j);
            //}
            //pb.choosedTetroIndex = a;

            foreach (int j in GameManager.instance.choosedTetroIndex)
            {
                Debug.Log(j);
            }


            GameManager.instance.currentStage++;
            SceneManager.LoadScene("Stage1");
        }
        catch
        {
            

            text.SetActive(true);
            
            Debug.Log("7개의 테트로미노 선택해야함");
        }

    }
}
