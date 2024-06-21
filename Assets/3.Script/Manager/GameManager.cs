using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    // 증감식 -> GetBlockIndex에 로드씬에 있음
    public int currentStage = 1;

    // 내가 고를 블럭
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
            // 해상도와 전체 화면 모드를 고정
            if (Screen.width != 1920 || Screen.height != 1080 || !Screen.fullScreen)
            {
                Screen.SetResolution(1920, 1080, false); 
               
            }
            yield return new WaitForSeconds(1); // 1초마다 확인
        }
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 실행할꺼
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
