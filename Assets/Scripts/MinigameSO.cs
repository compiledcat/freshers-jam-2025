using UnityEngine;

[CreateAssetMenu(fileName = "Minigame", menuName = "ScriptableObjects/MinigameScriptableObject", order = 1)]
public class MinigameSO : ScriptableObject
{
    public string title;
    public ControlsVisualiser.ControlState ControlState;
    public MinigameLevel linkedMinigame;
}
