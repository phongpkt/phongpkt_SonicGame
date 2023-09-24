using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float x;
    private float y;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            y = Random.Range(50, 62);
            collision.transform.position = new Vector2 (x, y);
        }
    }
}
