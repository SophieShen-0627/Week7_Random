using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    [Header("PlayerState")]
    public int PlayerLifeState = 0;        //0:playing; 1:lose; 2: win
    [SerializeField] ParticleSystem PlayerExplodeParticle;
    [SerializeField] GameObject Confetti;

    [Header("Movement")]
    public int ReverseMovement = 1;        //-1: reverse

    // super speed event will control RegMoveFOrce, BombMoveForce, RegMaxSpeed and BombMaxSpeed
    public float RegMoveForce = 3;
    public float BombMoveForce = 5;
    public float curMoveForce = 3;
    public float RegMaxSpeed = 8;
    public float BombMaxSpeed = 10;

    float curMaxSpeed = 8;
    public float RegDecelerateFactor = 0.05f;
    public float BombDecelerateFactor = 0.1f;
    public bool PlayerCanMove = true;
    float curDecFactor;
    Vector2 moveDir = Vector2.zero;
    Rigidbody2D rb;


    public enum PlayerNum { P1, P2, P3, P4 };
    [Header("Spawn")]
    public PlayerNum CurNum = PlayerNum.P1;


    [Header("Bomb")]
    public bool HasBomb = false;
    public bool CanPassBomb = true;
    bool contact = false;
    public float BombPassInterval = 0.5f;
    public float BouncingForce = 20;

    public GameObject Bomb;

    [Header("StateParticle")]
    [SerializeField] ParticleSystem ConfusedParticle;
    [SerializeField] GameObject SpeedUp;
    [SerializeField] ParticleSystem HoldBumb;


    float timer = 0;
    GameObject curOther = null;
    private BombManager bombmanager;
    private SpecialEvent specialeventtrigger;

    // ================================= Input System Related =================================== //
    InputActionAsset controls;
    InputActionMap playerInput;
    InputAction movement;
    InputAction restart;
    bool ifRestart;

    private void Awake()
    {
        controls = GetComponent<PlayerInput>().actions;
        rb = GetComponent<Rigidbody2D>();

        playerInput = controls.FindActionMap("Player Input");
        curMoveForce = RegMoveForce;
        curMaxSpeed = RegMaxSpeed;

        SpeedUp.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerNumUpdate();
        bombmanager = FindObjectOfType<BombManager>();
        specialeventtrigger = FindObjectOfType<SpecialEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        StateChange();
        if (GameManager.GameState)
        {
            BombPass();
            ContactWithOthers();
            BombReceiveCooldown();
        }

        PlayParticle();

        if (PlayerLifeState == 1)
        {
            PlayerLifeState = 3;
            Debug.Log("player dead");

            PlayerExplodeParticle.Play();
            PlayerExplodeParticle.transform.parent = null;


            gameObject.SetActive(false);
        }

        if (PlayerLifeState == 2)
        {
            PlayerLifeState = 3;
            StartCoroutine(ConfettiDelay());
        }

    }

    IEnumerator ConfettiDelay()
    {
        yield return new WaitForSeconds(0.6f);

        Confetti.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.WinSound);
    }
    private void FixedUpdate()
    {
        if (PlayerCanMove) Move();
        else rb.velocity = Vector2.zero;
    }

    private void PlayParticle()
    {
        if (ReverseMovement == -1 && !ConfusedParticle.isPlaying)
        {
            ConfusedParticle.Play();
        }
        else
        {
            ConfusedParticle.Stop();
        }

        if (rb.velocity.magnitude >= 10) SpeedUp.SetActive(true);
        else SpeedUp.SetActive(false);

        if (HasBomb) HoldBumb.Play();
        else HoldBumb.Stop();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>() * ReverseMovement;
    }
    public void RestartScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);

    }
    void PlayerNumUpdate()
    {
        switch (GameManager.CurPlayerNum)
        {
            case 0:
                CurNum = PlayerNum.P1;
                break;
            case 1:
                CurNum = PlayerNum.P2;
                break;
            case 2:
                CurNum = PlayerNum.P3;
                break;
            case 3:
                CurNum = PlayerNum.P4;
                break;

        }
    }
    void Move()
    {
        Vector2 move = moveDir * curMoveForce;
        rb.AddForce(move, ForceMode2D.Impulse);

        // Velocity Adjustment
        if (moveDir.magnitude <= 0.01f)
        {
            if (Mathf.Abs(rb.velocity.x) > curDecFactor)
            {
                int i = (rb.velocity.x >= 0) ? 1 : -1;
                rb.velocity -= new Vector2(curDecFactor * i, 0);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (Mathf.Abs(rb.velocity.y) > curDecFactor)
            {
                int i = (rb.velocity.y >= 0) ? 1 : -1;
                rb.velocity -= new Vector2(0, curDecFactor * i);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) > curMaxSpeed)
            {
                int i = (rb.velocity.x >= 0) ? 1 : -1;
                rb.velocity = new Vector2(curMaxSpeed * i, rb.velocity.y);
            }
            if (Mathf.Abs(rb.velocity.y) > curMaxSpeed)
            {
                int i = (rb.velocity.y >= 0) ? 1 : -1;
                rb.velocity = new Vector2(rb.velocity.x, curMaxSpeed * i);
            }
        }
    }
    void StateChange()
    {
        if (HasBomb)
        {
            curMaxSpeed = BombMaxSpeed;
            curMoveForce = BombMoveForce;
            curDecFactor = BombDecelerateFactor;
        }
        else
        {
            curMaxSpeed = RegMaxSpeed;
            curMoveForce = RegMoveForce;
            curDecFactor = RegDecelerateFactor;
        }
    }
    void BombPass()
    {
        if (HasBomb)
        {
            if (!Bomb.activeSelf)
                Bomb.SetActive(true);

        }
        else
        {
            if (Bomb.activeSelf)
                Bomb.SetActive(false);
        }
    }
    void ContactWithOthers()
    {
        if (contact && HasBomb && curOther.GetComponent<PlayerController>().CanPassBomb)
        {
            curOther.GetComponent<PlayerController>().HasBomb = true;
            HasBomb = false;
            CanPassBomb = false;

            // TODO: Global Counting Down Functions
            bombmanager.ChangeTimeDisplay();
            specialeventtrigger.TriggerSpecialEvent();

            Vector2 forceDir = (transform.position - curOther.transform.position).normalized * BouncingForce;
            curOther.GetComponent<Rigidbody2D>().AddForce(-forceDir, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().AddForce(forceDir, ForceMode2D.Impulse);
        }
    }
    void BombReceiveCooldown()
    {
        if (!CanPassBomb)
        {
            timer += Time.deltaTime;
            if (timer >= BombPassInterval)
            {
                CanPassBomb = true;
                timer = 0;
            }
        }
    }
    private void OnEnable()
    {
        movement = playerInput.FindAction("Movement");
        playerInput.FindAction("Restart").started += RestartScene;
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.FindAction("Restart").started -= RestartScene;

        playerInput.Disable();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (curOther == null)
            {
                curOther = other.gameObject;
            }
            if (curOther != null && curOther == other.gameObject)
            {
                contact = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (curOther == null)
            {
                curOther = other.gameObject;
            }
            if (curOther != null && curOther == other.gameObject)
            {
                contact = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            curOther = null;
            contact = false;
        }
    }
}
