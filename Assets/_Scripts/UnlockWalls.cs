using UnityEngine;

public class UnlockWalls : MonoBehaviour
{
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject button;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        barrier.SetActive(false);
        button.SetActive(false);
        Debug.Log("Barriers are gone...");
    }
}