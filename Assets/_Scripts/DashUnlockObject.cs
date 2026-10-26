using UnityEngine;

public class DashUnlockObject : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<PlatformControllerAdv>().UnlockDash();
            Debug.Log("Dash has been unlocked");
        }
    }
}
