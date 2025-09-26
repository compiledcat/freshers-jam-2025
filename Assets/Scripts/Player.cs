using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput PlayerInput;

    public Transform _livesDisplay;
    
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
        Destroy(_livesDisplay.GetChild(0).gameObject);
    }

    public bool IsAlive => lives > 0;

    private void Start()
    {
        GameManager.Instance.AddGameStartListener(() =>
        {
            hasFinishedLevel = false;
        });
    }
}