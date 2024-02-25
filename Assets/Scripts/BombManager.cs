using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BombManager : MonoBehaviour
{
    public TMP_Text TimeText;
    [SerializeField] LayerMask FloorLayer;
    [SerializeField] Color InitialColor = Color.white;
    [SerializeField] Color EndColor = Color.red;

    PlayerController[] players;
    [SerializeField] ParticleSystem ExplosionParticle;

    private float InitialTime = 40;
    private float RemainTime = 0;
    private bool HasExplodeBomb = false;

    // Start is called before the first frame update
    void Start()
    { 
        InitialTime = Random.Range(30.0f, 35f);
        TimeText.color = InitialColor;
        TimeText.text = (Mathf.FloorToInt(InitialTime) + 1).ToString();
        RemainTime = InitialTime;
    }

    // Update is called once per frame
    void Update()
    {
        RemainTime -= Time.deltaTime;

        if (RemainTime <= 0 && !HasExplodeBomb)
        {
            players = FindObjectsOfType<PlayerController>();
            foreach (var player in players)
            {
                player.PlayerCanMove = false;

                if (player.HasBomb)
                {
                    ExplodeBomb(player.transform);
                    HasExplodeBomb = true;
                    break;
                }
            }
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
