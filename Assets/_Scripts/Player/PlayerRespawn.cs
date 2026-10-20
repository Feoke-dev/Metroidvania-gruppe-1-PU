using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    Vector3 spawnPoint = Vector3.zero;
    Rigidbody2D playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        if (startPoint != null)
        {
            spawnPoint = startPoint.position;
        }
    }
    public void SetPlayerRespawn(Vector3 point)
    {
        spawnPoint = point;
    }

<<<<<<< Updated upstream
    public void RespawnPlayer()
=======
    public Vector3 GetPlayerRespawn()
    {
        return spawnPoint;
    }

    public void RewpawnPlayer()
>>>>>>> Stashed changes
    {
        transform.position = spawnPoint;
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
        }
    }
}
