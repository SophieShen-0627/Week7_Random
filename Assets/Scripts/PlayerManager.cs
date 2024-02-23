using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.iOS;
public class PlayerManager : MonoBehaviour
{
    public List<PlayerInput> PlayerInputs = new List<PlayerInput>();
    public List<Transform> StartingPoints;

    public PlayerInputManager PlayerControlManager;

    public List<Color> PlayerColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
        PlayerInputs.Add(playerInput);
        Transform playerParent = playerInput.transform;
        playerParent.position = StartingPoints[PlayerInputs.Count - 1].position;
        GameManager.CurPlayerNum += 1;
        playerInput.gameObject.GetComponent<SpriteRenderer>().color = PlayerColor[PlayerInputs.Count - 1];
        Debug.Log("Generated");
    }
    
}
