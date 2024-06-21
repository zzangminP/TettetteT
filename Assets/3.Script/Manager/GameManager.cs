using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    // ������ -> GetBlockIndex�� �ε���� ����
    public int currentStage = 1;

    // ���� �� ��
    public int[] choosedTetroIndex = new int[7];



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 144;
    }
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        for (int i = 0; i < choosedTetroIndex.Length; i++)
        {
            choosedTetroIndex[i] = i;
            Debug.Log(choosedTetroIndex[i]);
        }
        StartCoroutine(LockResolution());
    }
    System.Collections.IEnumerator LockResolution()
    {
        while (true)
        {
            // �ػ󵵿� ��ü ȭ�� ��带 ����
            if (Screen.width != 1920 || Screen.height != 1080 || !Screen.fullScreen)
            {
                Screen.SetResolution(1920, 1080, false); 
               
            }
            yield return new WaitForSeconds(1); // 1�ʸ��� Ȯ��
        }
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �����Ҳ�
        Init();
    }

    public void Init()
    {
        currentStage = 1;
        for (int i = 0; i < choosedTetroIndex.Length; i++)
        {
            choosedTetroIndex[i] = i;
            //Debug.Log(choosedTetroIndex[i]);
        }
    }


    /*
    void WhatStage()
    {

    }*/
}
