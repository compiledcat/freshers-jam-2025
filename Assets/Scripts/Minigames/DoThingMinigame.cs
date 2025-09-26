using UnityEngine;

namespace Minigames
{
    public class DoThingMinigame : MonoBehaviour
    {
        [SerializeField] private MinigameLevel _level;
        [SerializeField] private Transform _playerTransform;

        private void Start()
        {
            GameManager.Instance.AddGameEndListener(() =>
            {
                _level.Player.FinishLevel();
            });
        }

        private void Update()
        {
            if (!_level.InPlay || _level.Player.HasFinishedLevel) return;

            var pi = _level.Player.PlayerInput;
            var move = pi.actions["Move"].ReadValue<Vector2>();
            _playerTransform.Translate(move * (Time.deltaTime * 5));
            _playerTransform.localPosition = new Vector3(
                Mathf.Clamp(_playerTransform.localPosition.x, GameManager.Instance.LevelSize.x * -0.5f, GameManager.Instance.LevelSize.x * 0.5f),
                Mathf.Clamp(_playerTransform.localPosition.y, GameManager.Instance.LevelSize.y * -0.5f, GameManager.Instance.LevelSize.y * 0.5f),
                _playerTransform.localPosition.z);
        }
    }
}