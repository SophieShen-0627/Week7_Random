using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEvent : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int TimeForEachEvent = 5;
    [SerializeField] PlayerController[] players;

    [Header("Event1: mask")]
    [SerializeField] GameObject mask;

    [Header("Event3:superspeed")]
    [SerializeField] int SpeedMultiplier = 2;
    [SerializeField] float SizeMultiplier = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerController>();
        
    }

    public void TriggerSpecialEvent()
    {
        StartCoroutine(DoEvents());
    }

    IEnumerator DoEvents()
    {
        int caseSwitch = Random.Range(0, 3);

        switch(caseSwitch)
        {
            case 0:
                DoMaskEvent(true);
                yield return new WaitForSeconds(TimeForEachEvent);
                DoMaskEvent(false);
                break;

            case 1:
                ReverseInput(true);
                yield return new WaitForSeconds(TimeForEachEvent);
                ReverseInput(false);
                break;

            case 2:
                SuperSpeed(true);
                yield return new WaitForSeconds(TimeForEachEvent);
                SuperSpeed(false);
                break;
        }
    }

    void DoMaskEvent(bool state)
    {
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
        if (state)
        {
            foreach (var player in players)
            {
                player.curMoveForce *= SpeedMultiplier;
                player.RegMaxSpeed *= SpeedMultiplier;
                player.BombMaxSpeed *= SpeedMultiplier;
                player.transform.localScale *= SizeMultiplier;
            }
        }

        if (!state)
        {
            foreach (var player in players)
            {
                player.curMoveForce /= SpeedMultiplier;
                player.RegMaxSpeed /= SpeedMultiplier;
                player.BombMaxSpeed /= SpeedMultiplier;
                player.transform.localScale /= SizeMultiplier;
            }
        }
    }
}
