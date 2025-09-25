using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput PlayerInput;
    
    private uint lives = 3;
    private bool hasFinishedLevel = false;

    public bool HasFinishedLevel => hasFinishedLevel;

    public void FinishLevel()
    {
        hasFinishedLevel = true;
    }

    public void DecrementLife()
    {
        --lives;
        if (lives == 0)
        {
            //die
        }
    }

    public bool IsAlive()
    {
        return lives > 0;
    }

    private void Start()
    {
        GameManager.Instance.AddGameStartListener(() =>
        {
            hasFinishedLevel = false;
        });
    }
}