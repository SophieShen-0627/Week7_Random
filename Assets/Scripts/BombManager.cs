using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BombManager : MonoBehaviour
{
    public TMP_Text TimeText;
    public bool BombHasExplode = false;
    public bool DoExplosion = false;
    [SerializeField] LayerMask FloorLayer;
    [SerializeField] Color InitialColor = Color.white;
    [SerializeField] Color EndColor = Color.red;

    PlayerController[] players;
    [SerializeField] ParticleSystem ExplosionParticle;

    private float InitialTime = 40;
    private float RemainTime = 0;
    private bool HasExplodeBomb = false;
    private GameManager gamemanager;
    private bool StartGame = false;

    // Start is called before the first frame update
    void Start()
    { 
        InitialTime = Random.Range(30.0f, 35f);
        RemainTime = InitialTime;
        gamemanager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (gamemanager.GameStart && !StartGame)
        {
            StartGame = true;
            TimeText.color = InitialColor;
            TimeText.text = (Mathf.FloorToInt(InitialTime) + 1).ToString();
        }

        if (StartGame)
        {
            RemainTime -= Time.deltaTime;
        }*/

        //if (RemainTime <= 0 && !HasExplodeBomb)
        if (DoExplosion && !HasExplodeBomb)
        {
            players = FindObjectsOfType<PlayerController>();
            foreach (var player in players)
            {
                player.PlayerCanMove = false;

                if (player.HasBomb)
                {
                    ExplodeBomb(player.transform);
                    PlayerExplodeCount(player);
                    HasExplodeBomb = true;
                    break;
                }
            }
        }
    }

    void PlayerExplodeCount(PlayerController player)
    {
        foreach (var m_player in players)
        {
            if (Mathf.Abs(m_player.transform.position.x - player.transform.position.x) <= 0.5f || Mathf.Abs(m_player.transform.position.y - player.transform.position.y) <= 0.5f)
            {
                m_player.PlayerLifeState = 1;
            }
            else m_player.PlayerLifeState = 2;
        }
    }

    public void ChangeTimeDisplay()
    {
        TimeText.text = (Mathf.FloorToInt(RemainTime) + 1).ToString();
        if (RemainTime <= 10)
        {
            TimeText.color = EndColor;
        }
    }

    void ExplodeBomb(Transform Pos)
    {
        BombHasExplode = true;
        GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.BombExplosion);
        float left = GetPointHorizontal(Vector2.left, Pos);
        float right = GetPointHorizontal(Vector2.right, Pos);
        float up = GetPointVertical(Vector2.up, Pos);
        float down = GetPointVertical(Vector2.down, Pos);

        for (float i = left; i <= right; i++)
        {
            ParticleSystem temp = Instantiate(ExplosionParticle, new Vector3(i, Pos.position.y, Pos.position.z), Quaternion.identity);
            temp.Play();
        }

        for (float i = down; i <= up; i++)
        {
            ParticleSystem temp = Instantiate(ExplosionParticle, new Vector3(Pos.position.x, i, Pos.position.z), Quaternion.identity);
            temp.Play();
        }
    }

    private float GetPointHorizontal(Vector2 Direction, Transform pos)
    {
        RaycastHit2D HitPoint = Physics2D.Raycast(pos.position, Direction, 100f, FloorLayer);
        if (HitPoint.collider != null)
        {
            if (HitPoint.point.x <= transform.position.x)
                return pos.position.x - Mathf.RoundToInt(pos.position.x - HitPoint.point.x);
            else
                return pos.position.x + Mathf.RoundToInt(HitPoint.point.x - pos.position.x);
        }
        else return 0;
    }

    private float GetPointVertical(Vector2 Direction, Transform pos)
    {
        RaycastHit2D HitPoint = Physics2D.Raycast(pos.position, Direction, 100f, FloorLayer);
        if (HitPoint.collider != null)
        {
            if (HitPoint.point.y <= transform.position.y)
                return pos.position.y - Mathf.RoundToInt(pos.position.y - HitPoint.point.y);
            else
                return pos.position.y + Mathf.RoundToInt(HitPoint.point.y - pos.position.y);
        }
        else return 0;
    }
}
