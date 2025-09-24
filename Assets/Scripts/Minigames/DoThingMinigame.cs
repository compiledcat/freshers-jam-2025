using UnityEngine;

namespace Minigames
{
    public class DoThingMinigame : MonoBehaviour
    {
        [SerializeField] private MinigameLevel _level;
        [SerializeField] private Transform _playerTransform;

        private void Update()
        {
            var pi = _level.Player.PlayerInput;
            var move = pi.actions["Move"].ReadValue<Vector2>();
            _playerTransform.Translate(move * (Time.deltaTime * 5));
        }
    }
}