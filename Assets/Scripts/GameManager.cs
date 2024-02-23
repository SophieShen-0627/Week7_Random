using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int CurPlayerNum = 0;
    public static bool GameState = false; //true = start

    public bool GameStart = false;

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
        OnGameStart();
    }

    void OnGameStart()
    {
        if (!GameState && CurPlayerNum >= 3)
        {
            GameStart = true; //Debug用
            GameState = true;

            //TODO: Randomize Bomb Generate;

        }
    }


}
