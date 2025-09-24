using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<MinigameLevel> levels;
    private Camera _cam;

    [SerializeField] private TextMeshProUGUI _levelTitleText;

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
        foreach (Player p in PlayerManager.Instance.Players)
        {
            if (p.hasFinishedLevel)
            {
                p.DecrementLife();
            }
        }

        // todo
    }

    private void SetupLevels()
    {
        var randomLevel = levels[Random.Range(0, levels.Count)];

        var screenWidth = _cam.orthographicSize * 2 * _cam.aspect;
        for (var i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            var player = PlayerManager.Instance.Players[i];

            var level = Instantiate(randomLevel);
            if (i == 0)
            {
                level.transform.position = new Vector3(screenWidth * -0.25f, -_cam.orthographicSize * 2, 0);
            }
            else if (i == 1)
            {
                level.transform.position = new Vector3(screenWidth * 0.25f, -_cam.orthographicSize * 2, 0);
            }
            else
            {
                Debug.LogWarning("More than 2 players??? fucked up");
            }

            level.Player = player;
            Tween.PositionY(level.transform, 0, 1.0f, Ease.OutCubic, startDelay: 2.0f);
        }

        _levelTitleText.text = randomLevel.Title;
        _levelTitleText.rectTransform.anchoredPosition -= new Vector2(0, Screen.height);
        Sequence.Create()
            // from below to screen center
            .Group(Tween.UIAnchoredPositionY(_levelTitleText.rectTransform, 0, 1.0f, Ease.OutCubic))
            // from screen center to above
            .ChainDelay(1.0f)
            .Chain(Tween.UIAnchoredPositionY(
                _levelTitleText.rectTransform,
                _levelTitleText.rectTransform.anchoredPosition.y + Screen.height * 2, 1.0f, Ease.OutCubic
            ));
    }

    public void StartGame()
    {
        SetupLevels();
        // todo
    }
}