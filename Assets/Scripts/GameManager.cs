using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int CurPlayerNum = 0;
    public static bool PlayerRightSpawned = false;
    public static bool PlayerLeftSpawned = false;
    private void Awake()
    {
        CurPlayerNum = 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
