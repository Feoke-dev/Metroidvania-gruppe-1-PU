using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] GameObject activeVisual;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        SetCheckpoint(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        SetCheckpoint(other);
    }

    private void SetCheckpoint(Collider2D other)
    {
        PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
        if (playerRespawn == null && other.attachedRigidbody != null)
        {
            playerRespawn = other.attachedRigidbody.GetComponent<PlayerRespawn>();
        }
        if (playerRespawn == null)
        {
            playerRespawn = other.GetComponentInParent<PlayerRespawn>();
        }
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerRespawn == null && playerHealth != null)
        {
            playerRespawn = playerHealth.GetComponent<PlayerRespawn>();
            if (playerRespawn == null)
            {
                playerRespawn = playerHealth.GetComponentInParent<PlayerRespawn>();
            }
        }
        if (playerRespawn == null)
        {
            return;
        }

        Transform point = respawnPoint != null ? respawnPoint : transform;
        playerRespawn.SetPlayerRespawn(point.position);

        if (!activated)
        {
            activated = true;
            if (activeVisual != null)
            {
                activeVisual.SetActive(true);
            }
        }
    }
}
