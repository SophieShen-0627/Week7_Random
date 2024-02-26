using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEvent : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int TimeForEachEvent = 5;
    [SerializeField] PlayerController[] players;
    public bool InSpecialState = false;
    private int LastCase = 0;

    private bool LastEventHasFinished = true;
    private float timer = 0;   // for specialstate time count

    [Header("Event1: mask")]
    [SerializeField] GameObject mask;

    [Header("Event3:superspeed")]
    [SerializeField] int SpeedMultiplier = 2;
    [SerializeField] float SizeMultiplier = 0.4f;
    //store initial speed
    float bmf = 0;
    float rmf = 0;
    float rms = 0;
    float bms = 0;
    Vector3 scale = Vector3.zero;
    bool getInitialSpeedData = false;

    private void Update()
    {
        if (InSpecialState)
        {
            timer += Time.deltaTime;
            LastEventHasFinished = false;
        }

        if (timer >= 5)
        {
            InSpecialState = false;
            LastEventHasFinished = true;
        }
    }

    public void TriggerSpecialEvent()
    {
        StartCoroutine(DoEvents());
    }

    IEnumerator DoEvents()
    {
        players = FindObjectsOfType<PlayerController>();

        //用于替换主题曲
        InSpecialState = true;
        if (timer < 5) LastEventHasFinished = false;
        timer = 0;
        GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.TriggerEvent);

        if (!getInitialSpeedData)
        {
            bmf = players[0].BombMoveForce;
            rmf = players[0].RegMoveForce;
            rms = players[0].RegMaxSpeed;
            bms = players[0].BombMaxSpeed;
            scale = players[0].transform.localScale;
            getInitialSpeedData = true;
        }
        //防止同时播放两个special event
        DoMaskEvent(false);
        ReverseInput(false);
        SuperSpeed(false);

        int caseSwitch = Random.Range(0, 3);
        if (caseSwitch == LastCase)
        {
            caseSwitch = Random.Range(0, 3);
        }
        LastCase = caseSwitch;

        switch (caseSwitch)
        {
            case 0:
                DoMaskEvent(true);
                yield return new WaitForSeconds(TimeForEachEvent);
                if (LastEventHasFinished) DoMaskEvent(false);
                break;

            case 1:
                ReverseInput(true);
                yield return new WaitForSeconds(TimeForEachEvent);
                if(LastEventHasFinished) ReverseInput(false);
                break;

            case 2:
                SuperSpeed(true);
                yield return new WaitForSeconds(TimeForEachEvent);
                if(LastEventHasFinished) SuperSpeed(false);
                break;
        }
    }

    void DoMaskEvent(bool state)
    {
        Debug.Log("Do Mask Event");

        if (state)
        {
            mask.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }

        if (!state)
        {
            mask.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

    }

    void ReverseInput(bool state)
    {
        Debug.Log("Reverse input");

        if (state)
        {
            foreach (var player in players)
            {
                if (!player.HasBomb)
                {
                    player.ReverseMovement = -1;
                }
                else player.ReverseMovement = 1;
            }
        }

        if (!state)
        {
            foreach (var player in players) player.ReverseMovement = 1;
        }
    }

    void SuperSpeed(bool state)
    {
        Debug.Log("SuperSpeed");

        if (state)
        {
            foreach (var player in players)
            {
                player.BombMoveForce *= SpeedMultiplier;
                player.RegMoveForce *= SpeedMultiplier;
                player.RegMaxSpeed *= SpeedMultiplier;
                player.BombMaxSpeed *= SpeedMultiplier;
                player.transform.localScale *= SizeMultiplier;
            }
        }
        if (!state)
        {
            foreach (var player in players)
            {
                player.BombMoveForce = bmf;
                player.RegMoveForce = rmf;
                player.RegMaxSpeed = rms;
                player.BombMaxSpeed = bms;
                player.transform.localScale = scale;
            }
        }
    }
}
