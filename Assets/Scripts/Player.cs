using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput PlayerInput;
    
    private uint lives = 3;
    public bool hasFinishedLevel = false;

    public void DecrementLife()
    {
        --lives;
        if (lives == 0)
        {
            //die
        }
    }
}