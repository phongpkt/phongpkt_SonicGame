using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bulletRb;
    [SerializeField] private float speed;
    private GameObject target;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        Vector2 direction = (target.transform.position - transform.position).normalized * speed;
        bulletRb.velocity = new Vector2 (direction.x, direction.y);
        Invoke(nameof(OnDespawn), 4f);
    }
    private void OnDespawn()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = Cache.GetPlayer(collision);
            player.OnHit(20);
            OnDespawn();
        }
        else if (collision.CompareTag(Constants.PLAYER_Gold_TAG))
        {
            Player_Golden player = Cache.GetPlayerOnFinalMap(collision);
            player.OnHit(15);
            OnDespawn();
        }
    }
}
