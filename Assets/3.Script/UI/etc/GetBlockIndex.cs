using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetBlockIndex : MonoBehaviour
{
    public Image img;
    public string name;
    public Transform child;
    public Transform grandChild;
    public int[] a;
    
    private Transform pb;
    private PlayerBlocks pbs;
    public void CheckBlockIndex()
    {
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
            Debug.Log("7개의 테트로미노 선택해야함");
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
