using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    [Header("PlayerGenerate")]
    public List<PlayerInput> PlayerInputs = new List<PlayerInput>();
    public List<Transform> StartingPoints;

    public PlayerInputManager PlayerControlManager;

    public List<Color> PlayerColor;
    public Color OriUIColor;
    public List<Image> KeyboardLeftUI;
    public List<Image> KeyboardRightUI;
    public List<Image> Gamepad1UI;
    public List<Image> Gamepad2UI;

    [Header("PlayerInitialBombRandom")]
    public int RandomRangeMax = 4;
    public int RandomRangeMin = 0;
    bool randomed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RandomOnStart();
    }

    void RandomOnStart()
    {
        if (!randomed && GameManager.GameState)
        {
            randomed = true;
            int rand = Random.Range(RandomRangeMin, RandomRangeMax);
            PlayerInputs[rand].GetComponent<PlayerController>().HasBomb = true;
        }
    }

    // ========================== Generate Related ============================ //
    private void OnEnable()
    {
        PlayerControlManager.onPlayerJoined += AddPlayer;
    }
    private void OnDisable()
    {
        PlayerControlManager.onPlayerJoined -= AddPlayer;
    }
    public void AddPlayer(PlayerInput playerInput)
    {
        if (GameManager.CurPlayerNum < 4)
        {
            PlayerInputs.Add(playerInput);
            Transform playerParent = playerInput.transform;
            playerParent.position = StartingPoints[PlayerInputs.Count - 1].position;
            GameManager.CurPlayerNum += 1;
            playerInput.gameObject.GetComponent<SpriteRenderer>().color = PlayerColor[PlayerInputs.Count - 1];


            //UI Color Change
            if (playerInput.currentControlScheme == "Controller")
            {
                GamepadUIColorChange(PlayerColor[PlayerInputs.Count - 1]);
            }
            if (playerInput.devices[0].name == "Left")
            {
                SetColor(KeyboardLeftUI, PlayerColor[PlayerInputs.Count - 1]);
            }
            else if (playerInput.devices[0].name == "Right")
            {
                SetColor(KeyboardRightUI, PlayerColor[PlayerInputs.Count - 1]);
            }
        }
    }
    void GamepadUIColorChange(Color color)
    {
        if (canChangeColor(Gamepad1UI, color))
        {
            SetColor(Gamepad1UI, color);
        }
        else
        {
            SetColor(Gamepad2UI, color);
        }
    }
    bool canChangeColor(List<Image> images, Color color)
    {
        foreach (Image image in images)
        {
            if (image.color != OriUIColor)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    void SetColor(List<Image> images, Color color)
    {
        foreach (Image image in images)
        {
            image.color = color;
        }
    }
}
