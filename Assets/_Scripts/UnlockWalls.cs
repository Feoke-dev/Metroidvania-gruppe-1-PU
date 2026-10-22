using UnityEngine;

public class UnlockWalls : MonoBehaviour
{
    [SerializeField] private GameObject barrier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        barrier.SetActive(false);
    }
}