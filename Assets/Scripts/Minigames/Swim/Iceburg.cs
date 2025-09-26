using UnityEngine;

public class Iceburg : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _bounceForce = 5f;
    [SerializeField] bool _moveRight = true;


    private void Update()
    {
        _rb.linearVelocityX = _moveRight ? _moveSpeed : -_moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForceY(-_bounceForce, ForceMode2D.Impulse);
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            transform.localPosition = new Vector3(_moveRight ? -5 : 5, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
