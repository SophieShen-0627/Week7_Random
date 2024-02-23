using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float RegMoveForce = 3;
    public float BombMoveForce = 5;
    float curMoveForce = 3;
    public float RegMaxSpeed = 8;
    public float BombMaxSpeed = 10;
    float curMaxSpeed = 8;
    public float RegDecelerateFactor = 0.05f;
    public float BombDecelerateFactor = 0.1f;
    float curDecFactor;
    Rigidbody2D rb;
    Vector2 moveDir = Vector2.zero;

    public enum PlayerNum { P1, P2, P3, P4 };
    [Header("Spawn")]
    public PlayerNum CurNum = PlayerNum.P1;

    [Header("Bomb")]
    public bool ifHasBomb = false;
    public float BombPassInterval = 0.5f;
    float timer = 0;

    // =================================Input System Related=================================== //
    InputActionAsset controls;
    InputActionMap playerInput;
    InputAction movement;
    InputUser user;
    Keyboard keyboard;

    private void Awake()
    {
        controls = GetComponent<PlayerInput>().actions;
        playerInput = controls.FindActionMap("Player Input");
        rb = GetComponent<Rigidbody2D>();
        curMoveForce = RegMoveForce;
        curMaxSpeed = RegMaxSpeed;
        curDecFactor = RegDecelerateFactor;

    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerNumUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        StateChange();
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
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
        // Stop Input
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
        // Input
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
        if (ifHasBomb)
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
    private void OnEnable()
    {
        movement = playerInput.FindAction("Movement");
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
}
