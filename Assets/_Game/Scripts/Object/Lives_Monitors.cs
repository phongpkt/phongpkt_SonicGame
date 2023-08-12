 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives_Monitors : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            Destroy(this.gameObject);
            Player player = collision.gameObject.GetComponent<Player>();
            player.AddLives();
        }
    }
}
