using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int CurPlayerNum = 0;
    public static bool GameState = false; //true = start

    [Header("For Debugging")]
    public bool GameStart = false;
    public int MaxPlayerNum = 4;

    private float timer = 0;

    private void Awake()
    {
        CurPlayerNum = 0;
        GameState = GameStart;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        OnGameStart();
    }

    void OnGameStart()
    {
        if (!GameState && CurPlayerNum >= MaxPlayerNum)
        {
            GameStart = true; //Debug用
            GameState = true;

        }

        //测试用，只要过了5s且玩家数量大于2游戏就会开始
        if (!GameState && CurPlayerNum >= 2 && timer >= 5)
        {
            GameStart = true; //Debug用
            GameState = true;

        }
    }


}
