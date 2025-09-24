using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    private float timeToNextLevel; //The time remaining for the current level
    private float currentTimePerLevel; //The reset value that timeToNextLevel gets set to when the level starts (will decrease when the game speeds up)
    private const float startTimePerLevel = 8.0f; //The time that currentTimePerLevel starts at when the game starts up (before any speedups)

    private float levelsToNextSpeedup; //The number of levels left until the game speeds up
    private const float speedupFactor = 0.2f; //The amount that currentTimePerLevel speeds up by when levelsToNextSpeedup reaches 0 (0.2 is a 20% speedup)
    private const uint levelsPerSpeedup = 4; //The reset value that levelsToNextSpeedup gets set to when there is a speedup


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
    }


    private void Update()
    {
        timeToNextLevel -= Time.deltaTime;
        if (timeToNextLevel <= 0)
        {
            GameManager.AdvanceLevel();
            AdvanceLevel();
        }
    }
}
