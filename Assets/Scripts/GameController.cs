﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] commonObject;
    public GameObject[] specialObject;
    public GameObject[] otherSpecial;
    public GameObject Redbull;
    public bool isGameStart;
    public Transform[] commonSpawn;
    public bool[] isCommonEmpty;
    public Transform[] redbullSpawn;
    public Transform[] specialSpawn;
    public bool[] isSpecialEmpty;
    public bool[] isRedbullEmpty;

    public bool canCommonSpawn;
    public float CommonspawnTime = 2;
    public float specialSpawnTime = 3;
    public float redbullTime = 3;
    public int timeCount;
    public int timeCount2;
    public int timeCount3;
    public GameObject endGane;
    public Text showText;
    public GameObject p1;
    public GameObject p2;
    public GameObject programer1;
    public GameObject programer2;

    private Progresser progresser1;
    private Progresser progresser2;
    private ProgressHub progress1;
    private ProgressHub progress2;
    public GameObject Fade;   
    public GameObject ControlSelecter;
    public GameObject aboutJoy;
    public GameObject aboutKeyboard;
    public GameObject RightSelected;
    public GameObject LeftSelected;
    public bool isJoystick;
    private bool isInGame;
    private bool isGameEnd;
    private WaitForSeconds transitionTime;
    private Animator fadeAnim;
    private void Awake()
    {
        progress1 = p1.GetComponent<ProgressHub>();
        progress2 = p2.GetComponent<ProgressHub>();
        progresser1 = programer1.GetComponent<Progresser>();
        progresser2 = programer2.GetComponent<Progresser>();
        fadeAnim = Fade.GetComponent<Animator>();
        transitionTime = new WaitForSeconds(1f);
    }
    private void Start()
    {
        isCommonEmpty = new bool[commonSpawn.Length];

        for(int i =0;i<isCommonEmpty.Length;i++)
        {
            isCommonEmpty[i] = true;
        }
        isRedbullEmpty = new bool[redbullSpawn.Length];
        for (int i = 0; i < isRedbullEmpty.Length; i++)
        {
            isRedbullEmpty[i] = true;
        }
        isSpecialEmpty = new bool[specialSpawn.Length];
        for (int i = 0; i < isRedbullEmpty.Length; i++)
        {
            isSpecialEmpty[i] = true;
        }
        isGameStart = false;
        isInGame = false;
        RightSelected.SetActive(false);
        LeftSelected.SetActive(false);
        ControlSelecter.SetActive(true);
        aboutJoy.SetActive(false);
        aboutKeyboard.SetActive(false);
    }
    private void Update()
    {
        if (!isInGame)
        {
            if (ControlSelecter.activeSelf)
            {
                if(ActionInputController.getStartButton())
                {
                    isJoystick = true;
                    RightSelected.SetActive(true);
                    LeftSelected.SetActive(false);
                    fadeAnim.SetBool("isFadeOut", true);
                    StartCoroutine(TransitionFade());
                    ControlSelecter.SetActive(false);
                    aboutJoy.SetActive(true);
                    aboutKeyboard.SetActive(false);

                }
                else if(Input.GetKeyDown(KeyCode.Space))
                {
                    isJoystick = false;
                    RightSelected.SetActive(false);
                    LeftSelected.SetActive(true);
                    fadeAnim.SetBool("isFadeOut", true);
                    StartCoroutine(TransitionFade());
                    ControlSelecter.SetActive(false);
                    aboutJoy.SetActive(false);
                    aboutKeyboard.SetActive(true);
                }
            }
            else
            {
                if (isJoystick)
                {
                    if (ActionInputController.getBackButton())
                    {
                        isJoystick = false;
                        ControlSelecter.SetActive(true);
                        aboutJoy.SetActive(false);
                        aboutKeyboard.SetActive(false);
                    }
                    if (ActionInputController.getStartButton())
                    {
                        ControlSelecter.SetActive(false);
                        aboutJoy.SetActive(false);
                        aboutKeyboard.SetActive(false);
                        isGameStart = true;
                        isInGame = true;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        isJoystick = false;
                        ControlSelecter.SetActive(true);
                        aboutJoy.SetActive(false);
                        aboutKeyboard.SetActive(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        ControlSelecter.SetActive(false);
                        aboutJoy.SetActive(false);
                        aboutKeyboard.SetActive(false);
                        isGameStart = true;
                        isInGame = true;
                    }
                }
            }
        }
        if (isGameStart)
        {
            if (progress1.totalProgress >= 100)
            {
                showText.text = "Player1 is a winner!!!";
                isGameStart = false;
                progresser1.isWinner = true;
                isGameEnd = true;
            }
            if (progress2.totalProgress >= 100)
            {
                showText.text = "Player2 is a winner!!!";
                isGameStart = false;
                progresser2.isWinner = true;
                isGameEnd = true;
            }
            if (progress1.totalProgress >= 100 && progress2.totalProgress >= 100)
            {
                showText.text = "Draw";
                isGameStart = false;
                progresser1.isWinner = true;
                progresser2.isWinner = true;
                isGameEnd = true;
            }
        }
        if(isGameEnd)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if(Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene(0);
            }
            endGane.SetActive(true);
        }
        else
        {
            endGane.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void FixedUpdate()
    {
        if(timeCount < CommonspawnTime * 100)
        {
            timeCount += 1;
        }
        else
        {
            SpawnCommon();
            timeCount = 0;
        }

        if(timeCount3 < redbullTime * 100)
        {
            timeCount3 += 1;
        }
        else
        {
            SpawnRedBull();
            timeCount3 = 0;
        }
    }
    private void SpawnCommon()
    {
        int spawnIndex = -1 ;
        for(int i =0;i<isCommonEmpty.Length; i++)
        {
            if(isCommonEmpty[i])
            {
                canCommonSpawn = true;
            }
        }

        if(canCommonSpawn)
        {
            for(int i =0;i<isCommonEmpty.Length;i++)
            {
                if(isCommonEmpty[i])
                {
                    spawnIndex = i;
                }
            }
            if (spawnIndex != -1)
            {
                byte items = (byte)Random.Range(0, 4);
                Vector3 pos = commonSpawn[spawnIndex].position;
                GameObject temp = Instantiate(commonObject[items], pos, Quaternion.identity);
                isCommonEmpty[spawnIndex] = false;
                Item temp2 = temp.GetComponent<Item>();
                temp2.onIndex = spawnIndex;
                temp2.isSpecial = false;
            }
        }
    }
    private void SpawnRedBull()
    {
        if(isRedbullEmpty[0])
        {
            Vector3 pos = redbullSpawn[0].position;
            GameObject temp = Instantiate(Redbull, pos, Quaternion.identity);
            Item temp2 = temp.GetComponent<Item>();
            temp2.onIndex = 0;
            temp2.isOnRedBull = true;
            isRedbullEmpty[0] = false;
            temp2.isSpecial = false;
        }

        if(isRedbullEmpty[1])
        {
            Vector3 pos = redbullSpawn[1].position;
            GameObject temp = Instantiate(Redbull, pos, Quaternion.identity);
            Item temp2 = temp.GetComponent<Item>();
            temp2.onIndex = 1;
            temp2.isOnRedBull = true;
            isRedbullEmpty[1] = false;
            temp2.isSpecial = false;
        }
    }
    private IEnumerator TransitionFade()
    {
        yield return transitionTime;
        fadeAnim.SetBool("isFadeOut", false);
    }
}
