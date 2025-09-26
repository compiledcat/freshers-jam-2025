using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<MinigameSO> _levels;
    private Camera _cam;

    [SerializeField] private TextMeshProUGUI _levelTitleText;
    [SerializeField] private ControlsVisualiser _controlsVisualiser;
    [SerializeField] private RectTransform _introductionBox, _mainViewport, _backupViewport;

    private readonly List<MinigameLevel> _activeLevels = new();

    private readonly UnityEvent _onGameStart = new(); // timer begins
    private readonly UnityEvent _onGameEnd = new(); // timer ends, player controls stop, games could check if player completed objective

    public readonly Vector2 LevelSize = new(8, 9);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple instances of GameManager");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _cam = Camera.main;
    }


    public void AdvanceLevel()
    {
        _onGameEnd.Invoke();

        foreach (Player p in PlayerManager.Instance.Players)
        {
            if (!p.HasFinishedLevel)
            {
                p.DecrementLife();

                if (!p.IsAlive)
                {
                    // todo: player death
                    Application.Quit();
                }
            }
        }

        SetupLevels();

        // todo
    }

    private void SetupLevels()
    {
        List<GameObject> levelsToDestroy = new();
        for (var i = _activeLevels.Count - 1; i >= 0; i--)
        {
            var thisLevel = _activeLevels[i]; // capture before levels are cleared
            thisLevel.transform.position = new Vector3(thisLevel.transform.position.x, -thisLevel.transform.position.y);
            thisLevel.SetCameraTexture(i == 0 ? MinigameLevel.CameraTexture.BackupLeft : MinigameLevel.CameraTexture.BackupRight);
            levelsToDestroy.Add(thisLevel.gameObject);
        }
        _activeLevels.Clear();

        var randomLevel = _levels[Random.Range(0, _levels.Count)];
        for (var i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            var player = PlayerManager.Instance.Players[i];

            var level = Instantiate(randomLevel.linkedMinigame);
            if (i == 0)
            {
                level.transform.position = new Vector3(-LevelSize.x, -LevelSize.y, 0) * 100;
                level.SetCameraTexture(MinigameLevel.CameraTexture.Left);
            }
            else if (i == 1)
            {
                level.transform.position = new Vector3(LevelSize.x, -LevelSize.y, 0) * 100;
                level.SetCameraTexture(MinigameLevel.CameraTexture.Right);
            }
            else
            {
                Debug.LogWarning("More than 2 players??? fucked up");
            }

            level.Player = player;
            _activeLevels.Add(level);
        }

        float positionOffset = 1920 * Screen.height / Screen.width;
        _levelTitleText.text = randomLevel.title;
        _introductionBox.anchoredPosition = new Vector2(0, -positionOffset);
        _backupViewport.anchoredPosition = _mainViewport.anchoredPosition;
        _mainViewport.anchoredPosition = new Vector2(0, -positionOffset);
        Sequence.Create()
            // Carousel Up
            .Group(Tween.UIAnchoredPositionY(_backupViewport, positionOffset, 1.0f, Ease.OutCubic)) // from screen center to above
            .Group(Tween.UIAnchoredPositionY(_introductionBox, 0, 1.0f, Ease.OutCubic)) // from below to screen center
            
            .ChainCallback(() => {
                _controlsVisualiser.currentState = randomLevel.ControlState;
                _controlsVisualiser.StartFlash();
                foreach (var lvl in levelsToDestroy)
                {
                    Destroy(lvl);
                }
            })
            .ChainDelay(1.0f)

            // Carousel Up Again
            .Chain(Tween.UIAnchoredPositionY(_introductionBox, positionOffset, 1.0f, Ease.OutCubic))
            .Group(Tween.UIAnchoredPositionY(_mainViewport, 0, 1.0f, Ease.OutCubic)) // from below to screen center

            .ChainCallback(() => { _onGameStart.Invoke(); });
    }

    public void StartGame()
    {
        SetupLevels();
        // todo
    }

    public void AddGameStartListener(UnityAction call)
    {
        _onGameStart.AddListener(call);
    }

    public void RemoveGameStartListener(UnityAction call)
    {
        _onGameStart.RemoveListener(call);
    }

    public void AddGameEndListener(UnityAction call)
    {
        _onGameEnd.AddListener(call);
    }

    public void RemoveGameEndListener(UnityAction call)
    {
        _onGameEnd.RemoveListener(call);
    }
}