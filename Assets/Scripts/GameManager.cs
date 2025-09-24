using UnityEngine;
using System.Collections.Generic;

public static class GameManager
{
    public static List<Player> players;

    public static void AdvanceLevel()
    {
        foreach (Player p in players)
        {
            if (p.hasFinishedLevel)
            {
                p.DecrementLife();
            }
        }
    }

}
