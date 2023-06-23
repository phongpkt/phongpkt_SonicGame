using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = Cache.GetPlayer(collison);
            player.SavePoint();
        }
    }
}
