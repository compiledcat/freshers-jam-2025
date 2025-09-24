using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _playerInputManager;
    public Dictionary<PlayerInput, Player> Players = new();

    private void Awake()
    {
        _playerInputManager.playerJoinedEvent.AddListener(OnPlayerJoined);
        _playerInputManager.playerLeftEvent.AddListener(OnPlayerLeft);
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        var player = new Player();
        Players.Add(playerInput, player);
        Debug.Log("player joined: device " + playerInput.devices[0].description.manufacturer);
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Players.Remove(playerInput);
    }

    private void OnDestroy()
    {
        _playerInputManager.playerJoinedEvent.RemoveListener(OnPlayerJoined);
        _playerInputManager.playerLeftEvent.RemoveListener(OnPlayerLeft);
    }
}