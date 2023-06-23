using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class ZombieSight : MonoBehaviour
{
    public Zombie enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.PLAYER_TAG))
        {
            enemy.isChase = true;
            enemy.SetTarget(Cache.GetPlayer(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.PLAYER_TAG))
        {
            enemy.isChase = false;
            enemy.SetTarget(null);
        }
    }
}
