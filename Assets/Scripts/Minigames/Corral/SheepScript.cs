using UnityEngine;

public class SheepScript : MonoBehaviour
{
    [SerializeField] float runDistance;
    [SerializeField] float runSpeed;
    [SerializeField] Rigidbody2D rig;
    public Transform playerTransform;

    private void Update()
    {
        rig.linearVelocity = Vector2.zero;
        if (Vector2.SqrMagnitude(transform.position - playerTransform.position) < runDistance * runDistance)
        {
            Vector2 dir = (transform.position - playerTransform.position).normalized;
            rig.linearVelocity = runSpeed * dir;
            
            // turn to face running direction
            transform.localScale = new Vector3(Mathf.Sign(dir.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
