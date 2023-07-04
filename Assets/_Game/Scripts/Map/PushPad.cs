using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPad : MonoBehaviour
{
    [SerializeField] private float pushForce = 50f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.PLAYER_TAG))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * pushForce, ForceMode2D.Impulse);
        }
    }
}
