using UnityEngine;

public class Player : MonoBehaviour
{
    private uint lives = 3;
    public bool hasFinishedLevel;


    private void Start()
    {
        hasFinishedLevel = false;
    }

    public void DecrementLife()
    {
        --lives;
        if (lives == 0)
        {
            //die
        }
    }
}