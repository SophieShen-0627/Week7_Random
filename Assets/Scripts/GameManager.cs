using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int CurPlayerNum = 0;
    public static bool GameState = false; //true = start

    [Header("For Debugging")]
    public bool GameStart = false;
    public int MaxPlayerNum = 4;

    [Header("for UI")]
    [SerializeField] TMP_Text timertext;
    [SerializeField] GameObject UIwords;

    private float timer = 3;

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
        if (!GameState && CurPlayerNum >= MaxPlayerNum)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                timertext.text = (Mathf.FloorToInt(timer) + 1).ToString();
            }
            if (timer <= 0)
            {
                GameStart = true; //Debug用
                GameState = true;
                UIwords.SetActive(false);
            }

        }

    }


}
