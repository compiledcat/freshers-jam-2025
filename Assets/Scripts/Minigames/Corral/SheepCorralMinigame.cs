using UnityEngine;

namespace Minigames
{
    public class SheepCorralMinigame : MonoBehaviour
    {
        [SerializeField] private MinigameLevel _level;
        [SerializeField] private Rigidbody2D _playerRig;
        [SerializeField] private SheepScript _sheepPrefab;
        [SerializeField] private int _numSheep;
        [SerializeField] private Vector2 _spawnSize = new(6, 4);
        [SerializeField] private Vector2 _spawnCentre = new(0, 2);
        [SerializeField] private CorralScript corral;
        [SerializeField] private float _runSpeed = 2;

        private void Start()
        {
            Random.InitState(Time.frameCount); // So that the left and right players get the same sheep positions
            for (int i = 0; i < _numSheep; i++)
            {
                var sheep = Instantiate(_sheepPrefab, transform);
                sheep.playerTransform = _playerRig.transform;
                sheep.transform.localPosition = new Vector3(
                    Random.Range(_spawnCentre.x - _spawnSize.x * 0.5f, _spawnCentre.x + _spawnSize.x * 0.5f),
                    Random.Range(_spawnCentre.y - _spawnSize.y * 0.5f, _spawnCentre.y + _spawnSize.y * 0.5f),
                    0);
                if (Random.value > 0.5f)
                {
                    sheep.transform.localScale = new Vector3(-sheep.transform.localScale.x, sheep.transform.localScale.y, sheep.transform.localScale.z);
                }
            }

            GameManager.Instance.AddGameEndListener(() => { 
                if (corral.SheepCount == _numSheep) 
                { 
                    _level.Player.FinishLevel(); 
                } 
            });
        }

        private void Update()
        {
            if (!_level.InPlay || _level.Player.HasFinishedLevel) return;

            var pi = _level.Player.PlayerInput;
            var move = pi.actions["Move"].ReadValue<Vector2>();
            _playerRig.transform.localScale = new Vector3(Mathf.Sign(move.x) * Mathf.Abs(_playerRig.transform.localScale.x),
                _playerRig.transform.localScale.y, _playerRig.transform.localScale.z);
            _playerRig.linearVelocity = move * _runSpeed;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((Vector3)_spawnCentre + transform.position, (Vector3)_spawnSize);
        }
    }
}