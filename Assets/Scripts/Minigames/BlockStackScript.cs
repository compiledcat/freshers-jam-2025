using System.Collections.Generic;
using UnityEngine;

public class BlockStackScript : MonoBehaviour
{
    [SerializeField] MinigameLevel _level;

    [SerializeField] GameObject _squareBlock;
    [SerializeField] List<GameObject> _otherBlocks = new();
    [SerializeField] float _minBlockDim = 0.5f;
    [SerializeField] Vector2 _blockAreaRange = new(1f, 3f);
    [SerializeField] List<Color> _blockColors = new();
    [SerializeField] float _stackHeight = 5f;
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] Transform _player;
    GameObject _currentBlock;

    Random.State _randomState;

    void MakeBlock()
    {
        Random.state = _randomState;

        int randomIndex = Random.Range(0, _otherBlocks.Count + 3);
        if (randomIndex < _otherBlocks.Count)
        {
            _currentBlock = Instantiate(_otherBlocks[randomIndex], _player.position, Quaternion.identity, _player);
        }
        else
        {
            _currentBlock = Instantiate(_squareBlock, _player.position, Quaternion.identity, _player);
            float blockArea = Random.Range(_blockAreaRange.x, _blockAreaRange.y);
            float blockWidth = Random.Range(_minBlockDim, blockArea / _minBlockDim);
            _currentBlock.transform.localScale = new Vector3(blockWidth, blockArea / blockWidth, 1f);
        }

        _currentBlock.GetComponent<Collider2D>().enabled = false;
        _currentBlock.GetComponent<Rigidbody2D>().gravityScale = 0f;
        _currentBlock.GetComponent<SpriteRenderer>().color = _blockColors[Random.Range(0, _blockColors.Count)];
        _randomState = Random.state;
    }

    void DropBlock()
    {
        _currentBlock.transform.parent = transform;
        _currentBlock.GetComponent<Rigidbody2D>().gravityScale = 1f;
        _currentBlock.GetComponent<Collider2D>().enabled = true;
        MakeBlock();
    }

    void Start()
    {
        GameManager.Instance.AddGameEndListener(() =>
        {
            // Todo: delay before checking for win

            RaycastHit2D hit = Physics2D.Linecast(
                (Vector2)transform.position + new Vector2(-GameManager.LevelSize.x / 2, _stackHeight),
                (Vector2)transform.position + new Vector2(GameManager.LevelSize.x / 2, _stackHeight));
            if (hit.collider != null)
            {
                _level.Player.FinishLevel();
            }
        });

        Random.InitState(Time.frameCount);
        _randomState = Random.state;
        MakeBlock();
    }

    void Update()
    {
        if (!_level.InPlay || _level.Player.HasFinishedLevel) return;

        var pi = _level.Player.PlayerInput;
        var move = pi.actions["Move"].ReadValue<Vector2>();
        _player.localPosition += _moveSpeed * Time.deltaTime * new Vector3(move.x, 0, 0);
        _player.localPosition = new Vector3(
            Mathf.Clamp(_player.localPosition.x, -GameManager.LevelSize.x / 2 + 0.5f, GameManager.LevelSize.x / 2 - 0.5f),
            _player.localPosition.y,
            _player.localPosition.z);

        if (pi.actions["Select"].WasPressedThisFrame())
        {
            DropBlock();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            new Vector3(-GameManager.LevelSize.x / 2, _stackHeight) + transform.position, 
            new Vector3(GameManager.LevelSize.x / 2, _stackHeight) + transform.position);

        Gizmos.DrawWireCube(transform.position, new Vector3(Mathf.Sqrt(_blockAreaRange.x), Mathf.Sqrt(_blockAreaRange.x), 1f));
        Gizmos.DrawWireCube(transform.position, new Vector3(Mathf.Sqrt(_blockAreaRange.y), Mathf.Sqrt(_blockAreaRange.y), 1f));
        Gizmos.DrawWireCube(transform.position, new Vector3(_minBlockDim, _blockAreaRange.x / _minBlockDim, 1f));
        Gizmos.DrawWireCube(transform.position, new Vector3(_minBlockDim, _blockAreaRange.y / _minBlockDim, 1f));
    }
}
