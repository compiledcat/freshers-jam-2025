using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private Transform _livesDisplayL, _livesDisplayR;
    public List<Player> Players = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple instances of PlayerManager");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _playerInputManager.playerJoinedEvent.AddListener(OnPlayerJoined);
        _playerInputManager.playerLeftEvent.AddListener(OnPlayerLeft);
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Players.Add(playerInput.GetComponent<Player>());
        Debug.Log($"player {playerInput.playerIndex} joined");

        playerInput.GetComponent<Player>()._livesDisplay = playerInput.playerIndex == 0 ? _livesDisplayL : _livesDisplayR;

        if (Players.Count == 2)
        {
            GameManager.Instance.StartGame();
        }
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Players.Remove(playerInput.GetComponent<Player>());
        Debug.Log($"player {playerInput.playerIndex} left");
    }

    private void OnDestroy()
    {
        _playerInputManager.playerJoinedEvent.RemoveListener(OnPlayerJoined);
        _playerInputManager.playerLeftEvent.RemoveListener(OnPlayerLeft);
    }
}