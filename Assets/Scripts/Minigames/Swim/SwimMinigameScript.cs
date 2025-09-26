using UnityEngine;

public class SwimMinigameScript : MonoBehaviour
{
    [SerializeField] MinigameLevel _level;
    [SerializeField] private Rigidbody2D _playerRig;
    [SerializeField] Transform _cam;
    [SerializeField] float _finishY = 20f;
    [SerializeField] float _swimForce = 5f;

    bool justStarted = true;
    bool nextIsLeft = true;

    // Update is called once per frame
    void Update()
    {
        _cam.localPosition = new Vector3(0, Mathf.Clamp(_playerRig.transform.localPosition.y, -4.5f, 4.5f), _cam.localPosition.z);

        if (!_level.InPlay || _level.Player.HasFinishedLevel) return;

        var pi = _level.Player.PlayerInput;
        bool leftPressed = pi.actions["BumperL"].WasPressedThisFrame();
        bool rightPressed = pi.actions["BumperR"].WasPressedThisFrame();
        bool moveForward = false;

        if (justStarted)
        {
            if (leftPressed || rightPressed)
            {
                justStarted = false;
                nextIsLeft = !leftPressed; // If left pressed, next is right
                moveForward = true;
            }
        }
        else if ((nextIsLeft && leftPressed) || (!nextIsLeft && rightPressed))
        {
            moveForward = true;
            nextIsLeft = !nextIsLeft;
        }

        if (moveForward)
        {
            _playerRig.AddForceY(_swimForce, ForceMode2D.Impulse);
        }

        if (_playerRig.transform.localPosition.y >= _finishY)
        {
            _level.Player.FinishLevel();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-10, _finishY) + transform.position, new Vector3(10, _finishY) + transform.position);
    }
}
