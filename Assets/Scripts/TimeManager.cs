using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    private float timeToNextLevel; //The time remaining for the current level

    private float currentTimePerLevel; //The reset value that timeToNextLevel gets set to when the level starts (will decrease when the game speeds up)

    private const float startTimePerLevel = 8.0f; //The time that currentTimePerLevel starts at when the game starts up (before any speedups)

    private float levelsToNextSpeedup; //The number of levels left until the game speeds up

    private const float speedupFactor = 0.2f; //The amount that currentTimePerLevel speeds up by when levelsToNextSpeedup reaches 0 (0.2 is a 20% speedup)

    private const uint levelsPerSpeedup = 4; //The reset value that levelsToNextSpeedup gets set to when there is a speedup

    [SerializeField] private TextMeshProUGUI _timeText;

    private bool _inPlay = false;

    public void AdvanceLevel()
    {
        --levelsToNextSpeedup;
        if (levelsToNextSpeedup == 0)
        {
            levelsToNextSpeedup = levelsPerSpeedup;

            //Speed up
            currentTimePerLevel -= currentTimePerLevel * speedupFactor;
        }

        timeToNextLevel = currentTimePerLevel;
    }

    private void Start()
    {
        currentTimePerLevel = startTimePerLevel;
        timeToNextLevel = currentTimePerLevel;

        GameManager.Instance.AddGameStartListener(() =>
        {
            _inPlay = true;
        });
    }


    private void Update()
    {
        if (!_inPlay) return;

        timeToNextLevel -= Time.deltaTime;
        _timeText.text = timeToNextLevel.ToString("F1");
        if (timeToNextLevel <= 0)
        {
            _inPlay = false;
            GameManager.Instance.AdvanceLevel();
            AdvanceLevel();
        }
    }
}