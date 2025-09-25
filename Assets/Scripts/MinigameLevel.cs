using UnityEngine;

public class MinigameLevel : MonoBehaviour
{
    public string Title;
    public Player Player;

    bool _inPlay = false;

    public bool InPlay => _inPlay;


    private void Start()
    {
        GameManager.Instance.AddGameStartListener(() =>
        {
            _inPlay = true;
        });

        GameManager.Instance.AddGameEndListener(() =>
        {
            _inPlay = false;
        });
    }
}
