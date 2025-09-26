using UnityEngine;

public class CorralScript : MonoBehaviour
{
    private int _sheepCount;
    public int SheepCount => _sheepCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SheepScript>(out _))
        {
            _sheepCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SheepScript>(out _))
        {
            _sheepCount--;
        }
    }
}
